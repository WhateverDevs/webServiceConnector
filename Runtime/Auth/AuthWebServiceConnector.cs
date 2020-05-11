using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Serialization;
using Zenject;

namespace WhateverDevs.WebServiceConnector.Runtime.Auth
{
    /// <summary>
    /// Implementation of a web service connector with authentication.
    /// </summary>
    public class AuthWebServiceConnector : Loggable<AuthWebServiceConnector>, IAuthWebServiceConnector
    {
        /// <summary>
        /// Reference to the configuration manager.
        /// </summary>
        [Inject]
        public IConfigurationManager ConfigurationManager;

        /// <summary>
        /// Reference to the json serializer.
        /// </summary>
        [Inject]
        public ISerializer<string> JsonSerializer;

        /// <summary>
        /// Flag to know if we are logged in.
        /// </summary>
        private bool loggedIn;

        /// <summary>
        /// Login token.
        /// </summary>
        private string sessionToken;

        /// <summary>
        /// Reference to the configuration.
        /// </summary>
        private AuthWebServiceConnectorConfig Config
        {
            get
            {
                if (config == null) ConfigurationManager.GetConfiguration(out config);
                return config;
            }
        }

        /// <summary>
        /// Backfield for Config.
        /// </summary>
        private AuthWebServiceConnectorConfig config;

        /// <summary>
        /// Are we logged in?
        /// </summary>
        public bool LoggedIn() => loggedIn;

        /// <summary>
        /// Login to the service.
        /// </summary>
        /// <param name="email">Login credentials.</param>
        /// <param name="password">Login credentials.</param>
        /// <param name="resultCallback">Login result.</param>
        public void Login(string email, string password, Action<LoginResult> resultCallback)
        {
            GetLogger().Info("Attempting to login with email \"" + email + "\"");
            
            UnityWebRequest request = new UnityWebRequest(Config.Uri + Config.LoginUri, UnityWebRequest.kHttpVerbPOST)
                                      {
                                          uploadHandler =
                                              new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonSerializer
                                                                                             .To(new LoginParameters()
                                                                                                 {
                                                                                                     email = email,
                                                                                                     password = password
                                                                                                 })))
                                              {
                                                  contentType = "application/json"
                                              },
                                          downloadHandler = new DownloadHandlerBuffer()
                                      };

            request.SetRequestHeader("Content-Type", "application/json");

            CoroutineRunner.Instance.RunRoutine(Login(request, resultCallback));
        }

        /// <summary>
        /// Login to the service.
        /// </summary>
        /// <param name="loginRequest">Whe web request.</param>
        /// <param name="resultCallback">Login result.</param>
        private IEnumerator Login(UnityWebRequest loginRequest, Action<LoginResult> resultCallback)
        {
            yield return loginRequest.SendWebRequest();

            bool success = !(loginRequest.isNetworkError || loginRequest.isHttpError);

            if (success)
            {
                sessionToken = JsonSerializer
                              .From<LoginToken>(Encoding.UTF8.GetString(loginRequest.downloadHandler.data))
                              .token;

                loggedIn = true;
                resultCallback.Invoke(LoginResult.Success);
            }
            else
            {
                if (loginRequest.responseCode == 401)
                {
                    GetLogger().Error("Error 401: Unauthorized.");
                    resultCallback.Invoke(LoginResult.Unauthorized);
                }
                else
                {
                    GetLogger().Error("Web request response: " + loginRequest.responseCode + ".");
                    GetLogger().Error("Web request error: " + loginRequest.error + ".");
                    resultCallback.Invoke(LoginResult.Error);
                }

                loggedIn = false;
            }
        }

        public void Logout(Action<bool> resultCallback) => throw new NotImplementedException();

        public void Register(string username, string email, string password, Action<bool> resultCallback) =>
            throw new NotImplementedException();

        public void PostForJsonTextWithoutParams(string uri, Action<bool, string> resultCallback) =>
            throw new NotImplementedException();

        /// <summary>
        /// Class representing the login parameters.
        /// </summary>
        [Serializable]
        private class LoginParameters
        {
            // ReSharper disable InconsistentNaming
            // ReSharper disable NotAccessedField.Local
            public string email;

            public string password;
        }

        /// <summary>
        /// Class representing the login token.
        /// </summary>
        [Serializable]
        private class LoginToken
        {
            public string token;
            // ReSharper restore InconsistentNaming
            // ReSharper restore NotAccessedField.Local
        }
    }
}