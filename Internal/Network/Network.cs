using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal static class Network
    {
        private static readonly HttpClient Client;

        static Network()
        {
            Client = new HttpClient();

            var acceptLanguage = new StringWithQualityHeaderValue(BridgeConfig.IsMainland ? "zh" : "en");
            Client.DefaultRequestHeaders.AcceptLanguage.Add(acceptLanguage);
            Client.DefaultRequestHeaders.Add("mainland", BridgeConfig.IsMainland ? "true" : "false");
        }

        internal static async void Send(RequestBase request, IRequestCompletedHandler completedHandler)
        {
            CancellationTokenSource timeOutToken = new CancellationTokenSource(request.TimeOut);

            try
            {
                HttpRequestMessage requestMessage = CreateRequest(request);

                Logger.Log($"开始发起请求: {requestMessage.RequestUri.AbsoluteUri}");
                Logger.Log($"请求 body 参数: {requestMessage.Content}");

                var response = await Client.SendAsync(requestMessage, timeOutToken.Token);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Logger.Log($"请求成功！<color=green>{body}</color>");
                    completedHandler.OnSuccessHandler(body);
                }
                else
                {
                    Logger.LogError($"请求失败！状态码: {response.StatusCode}");
                    completedHandler.OnFailureHandler(new ServiceError((int)response.StatusCode, "发起请求失败"));
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError($"请求发生异常: {ex.Message}");
                completedHandler.OnFailureHandler(new ServiceError(-1000, $"请求发生异常: {ex.Message}"));
            }
            catch (TaskCanceledException ex)
            {
                if (timeOutToken.Token.IsCancellationRequested)
                {
                    Logger.LogError("请求超时: " + ex.Message);
                    completedHandler.OnFailureHandler(new ServiceError(-1003, $"请求超时"));
                }
                else
                {
                    Logger.LogError($"未知异常. {ex.Message}");
                    completedHandler.OnFailureHandler(new ServiceError(-1, $"未知异常. {ex.Message}"));
                }
            }
        }

        private static HttpRequestMessage CreateRequest(RequestBase request)
        {

            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = request.Method;

            var parameters = request.Parameters();

            if (request.AppID)
            {
                parameters.Add("app_id", BridgeConfig.AppID);
            }

            if (request.Method == HttpMethod.Post)
            {
                message.RequestUri = new Uri($"{request.BaseURL}{request.Path}");
                string bodyString = JsonConvert.SerializeObject(parameters);

                message.Content = new StringContent(bodyString);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                StringBuilder queryBuilder = new StringBuilder();
                foreach (var parameter in parameters)
                {
                    string encodedKey = Uri.EscapeDataString(parameter.Key);
                    string encodedValue = Uri.EscapeDataString(parameter.Value.ToString());

                    if (queryBuilder.Length > 0)
                    {
                        queryBuilder.Append("&");
                    }
                    else
                    {
                        queryBuilder.Append("?");
                    }

                    queryBuilder.AppendFormat("{0}={1}", encodedKey, encodedValue);
                }

                message.RequestUri = new Uri($"{request.BaseURL}{request.Path}{queryBuilder}");
            }

            return message;
        }

    }
}

