using System;
using UnityEngine;

namespace SoFunny.FunnySDKPreview
{
    /// <summary>
    /// Represents an access token which is used to access the FUNNY Platform. Most API calls
    /// to the FUNNY Platform require an access token as evidence of successful authorization.
    /// A valid access token is issued after the user grants your app the permissions that
    /// your app requests. An access token is bound to permissions (scopes) that define the
    /// API endpoints that you can access. Choose the permissions for your channel in the
    /// FUNNY Developers site and set them in the login method used in your app.
    ///
    /// An access token expires after a certain period. `ExpiresIn` specifies the time until
    /// which this access token expires.
    ///
    /// By default, the FUNNY SDK stores access tokens in a secure place on the device running
    /// your app and obtains authorization when you access the FUNNY Platform through the
    /// framework request methods.
    ///
    /// Don't try to create an access token yourself. You can get the stored access token with
    /// fewer properties with `FunnySDK.CurrentAccessToken`.
    /// </summary>
    ///
    [Serializable]
    public class AccessToken
    {
#if UNITY_ANDROID
#pragma warning disable 0649
        [SerializeField] private WapperToken accessToken;
#pragma warning restore 0649
        /// <summary>
        /// The value of the access token.
        /// </summary>
        ///
        public string Value { get { return accessToken.access_token; } }

        /// <summary>
        /// Number of seconds until the access token expires.
        /// </summary>
        ///
        public long ExpiresIn { get { return accessToken.expires_in; } }

        /// <summary>
        /// The raw string value of the ID token bound to the access token. The
        /// value exists only if the access token is obtained with the "openID"
        /// permission.
        /// </summary>
        ///
        public string IdTokenRaw { get { return accessToken.id_token; } }

        /// <summary>
        /// The refresh token bound to the access token.
        /// </summary>
        ///
        public string RefreshToken { get { return accessToken.refresh_token; } }

        /// <summary>
        /// Permissions granted by the user.
        /// </summary>
        ///
        public string Scope { get { return accessToken.scope; } }

        /// <summary>
        /// The expected authorization type when this token is used in a request
        /// header. Fixed to "Bearer" for now.
        /// </summary>
        ///
        public string TokenType { get { return accessToken.token_type; } }

#else
#pragma warning disable 0649
        [SerializeField] private string access_token;
        [SerializeField] private long expires_in;
        [SerializeField] private string id_token;
        [SerializeField] private string refresh_token;
        [SerializeField] private string scope;
        [SerializeField] private string token_type;

#pragma warning restore 0649

        /// <summary>
        /// The value of the access token.
        /// </summary>
        ///
        public string Value { get { return access_token; } }

        /// <summary>
        /// Number of seconds until the access token expires.
        /// </summary>
        ///
        public long ExpiresIn { get { return expires_in; } }

        /// <summary>
        /// The raw string value of the ID token bound to the access token. The
        /// value exists only if the access token is obtained with the "openID"
        /// permission.
        /// </summary>
        ///
        public string IdTokenRaw { get { return id_token; } }

        /// <summary>
        /// The refresh token bound to the access token.
        /// </summary>
        ///
        public string RefreshToken { get { return refresh_token; } }

        /// <summary>
        /// Permissions granted by the user.
        /// </summary>
        ///
        public string Scope { get { return scope; } }

        /// <summary>
        /// The expected authorization type when this token is used in a request
        /// header. Fixed to "Bearer" for now.
        /// </summary>
        ///
        public string TokenType { get { return token_type; } }
#endif


    }


    [Serializable]
    internal class WapperToken
    {

#pragma warning disable 0649
        [SerializeField] internal string access_token;
        [SerializeField] internal long expires_in;
        [SerializeField] internal string id_token;
        [SerializeField] internal string refresh_token;
        [SerializeField] internal string scope;
        [SerializeField] internal string token_type;

#pragma warning restore 0649

    }

}

