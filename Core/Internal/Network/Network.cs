using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 请求结果委托
    /// </summary>
    /// <param name="body"></param>
    /// <param name="error"></param>
    internal delegate void RequestCompletedHandler(string body, ServiceError error);

    /// <summary>
    /// 网络请求类
    /// </summary>
    internal static class Network
    {
        private static readonly HttpClient Client;

        static Network()
        {
            if (BridgeConfig.IsMainland)
            {
                Client = new HttpClient();
            }
            else
            {
#if UNITY_2021_1_OR_NEWER
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None) return true;

                    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
                    {
                        Uri BaseUri = new Uri(BridgeConfig.BaseURL);

                        // 根证书不受信任时，判断请求 Host 是否匹配
                        return sender.RequestUri.Host == BaseUri.Host;
                    }

                    return false;
                };

                Client = new HttpClient(clientHandler);
#else
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {

                    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None) return true;

                    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
                    {
                        // 根证书不受信任时，判断请求 Host 是否匹配
                        Uri BaseUri = new Uri(BridgeConfig.BaseURL);

                        if (sender is HttpWebRequest request)
                        {
                            return request.RequestUri.Host == BaseUri.Host;
                        }
                        else if (sender is HttpRequestMessage httpRequest)
                        {
                            return httpRequest.RequestUri.Host == BaseUri.Host;
                        }
                    }

                    return false;
                };

                Client = new HttpClient();
#endif

            }

            var acceptLanguage = new StringWithQualityHeaderValue(BridgeConfig.IsMainland ? "zh" : "en");
            Client.DefaultRequestHeaders.AcceptLanguage.Add(acceptLanguage);
            Client.DefaultRequestHeaders.Add("mainland", BridgeConfig.IsMainland ? "true" : "false");
            Client.DefaultRequestHeaders.Add("device_category", "pc");

            if (!string.IsNullOrEmpty(BridgeConfig.Host))
            {
                Client.DefaultRequestHeaders.Host = BridgeConfig.Host;
            }
        }

        private static async void Send(HttpRequestMessage request, TimeSpan timeOut, RequestCompletedHandler completedHandler)
        {
            CancellationTokenSource timeOutToken = new CancellationTokenSource(timeOut);

            try
            {
                // 发起请求
                var response = await Client.SendAsync(request, timeOutToken.Token);
                // 获取响应体
                var responseBody = await response.Content.ReadAsStringAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Logger.Log($"请求成功！{responseBody}", Logger.ColorStyle.Green);
                        completedHandler(responseBody, null);
                        break;
                    case HttpStatusCode.BadRequest:
                        Logger.LogError($"请求失败！StatusCode = 400, Response = {responseBody}");
                        var resBody = JObject.Parse(responseBody);
                        int errorCode = resBody.Value<int>("code");
                        string errorMessage = resBody.Value<string>("message");
                        ServiceError error = new ServiceError(errorCode, errorMessage);
                        completedHandler(null, error);
                        break;
                    case HttpStatusCode.Unauthorized:

                        // 刷新 Token 机制
                        SSOToken newToken = await RefreshSSOTokenHandler();

                        if (newToken is null)
                        {
                            Logger.LogError($"请求失败！StatusCode = 401, Response = {responseBody}");
                            completedHandler(null, ServiceError.Make(ServiceErrorType.InvalidAccessToken));
                        }
                        else
                        {
                            // 更新当前 Token 记录
                            FunnyDataStore.UpdateTokenAndRecord(newToken);

                            // 继续之前请求
                            HttpRequestMessage newRequest = new HttpRequestMessage(request.Method, request.RequestUri);
                            newRequest.Content = request.Content;
                            newRequest.Content.Headers.ContentType = request.Content.Headers.ContentType;
                            newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken.Value);

                            Send(newRequest, timeOut, completedHandler);
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        Logger.LogError($"请求失败！StatusCode = 500, Response = {responseBody}");
                        completedHandler(null, ServiceError.Make(ServiceErrorType.ServerOccurredFailed));
                        break;
                    default:
                        Logger.LogError($"请求发生未知错误！StatusCode: {(int)response.StatusCode} Response: {responseBody}");
                        completedHandler(null, ServiceError.Make(ServiceErrorType.UnknownError));
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError($"请求失败: {ex.Message}");
                completedHandler(null, new ServiceError(-1, ex.Message));
            }
            catch (TaskCanceledException ex)
            {
                if (timeOutToken.Token.IsCancellationRequested)
                {
                    Logger.LogError("请求超时: " + ex.Message);
                    completedHandler(null, ServiceError.Make(ServiceErrorType.ConnectToServerFailed));
                }
                else
                {
                    Logger.LogError($"未知异常. {ex.Message}");
                    completedHandler(null, ServiceError.Make(ServiceErrorType.UnknownError));
                }
            }
            catch (JsonException ex)
            {
                Logger.LogError($"数据解析失败 - {ex.Message}");
                completedHandler(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
            }
        }

        internal static void Send(RequestBase request, RequestCompletedHandler completedHandler)
        {

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                completedHandler?.Invoke(null, ServiceError.Make(ServiceErrorType.ConnectToServerFailed));
                return;
            }

            HttpRequestMessage requestMessage = CreateRequest(request);

            Logger.Log($"开始发起请求: {requestMessage.Method} - {requestMessage.RequestUri.AbsoluteUri}");
            var parameters = JsonConvert.SerializeObject(request.Parameters());

            Logger.Log($"请求参数: {parameters}");

            Send(requestMessage, request.TimeOut, completedHandler);
        }

        private static HttpRequestMessage CreateRequest(RequestBase request)
        {

            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = request.Method;

            if (!string.IsNullOrEmpty(request.Token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
            }

            var parameters = request.Parameters();

            if (request.AppID)
            {
                parameters.Add("app_id", BridgeConfig.AppID);
            }

            if (request.DeviceInfo)
            {
                parameters.Add("device_id", SystemInfo.deviceUniqueIdentifier);
                parameters.Add("device_name", SystemInfo.deviceName);
            }

            if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
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

        private static async Task<SSOToken> RefreshSSOTokenHandler()
        {
            PostRefreshTokenRequest request = new PostRefreshTokenRequest(FunnyDataStore.Current);
            HttpRequestMessage requestMessage = CreateRequest(request);
            CancellationTokenSource timeOutToken = new CancellationTokenSource(request.TimeOut);

            var response = await Client.SendAsync(requestMessage, timeOutToken.Token);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                SSOToken newToken = JsonConvert.DeserializeObject<SSOToken>(responseBody);
                return newToken;
            }

            return null;
        }

    }
}

