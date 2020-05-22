using System;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Configuration;
using Zenject;

namespace WhateverDevs.WebServiceConnector.Runtime.NonAuth
{
    /// <summary>
    /// Web service connector that sends requests to a service that doesn't require auth.
    /// </summary>
    public class NonAuthServiceConnector : Loggable<NonAuthServiceConnector>, INonAuthWebServiceConnector
    {
        /// <summary>
        /// Reference to the configuration manager.
        /// </summary>
        [Inject]
        public IConfigurationManager ConfigurationManager;

        /// <summary>
        /// Reference to the configuration.
        /// </summary>
        private NonAuthServiceConnectorConfig Config
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
        private NonAuthServiceConnectorConfig config;

        /// <summary>
        /// Ask the server for a json.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        public void GetJsonWithoutParams(string uri, Action<bool, string> resultCallback) =>
            CoroutineRunner.Instance.RunRoutine(GetJsonWithoutParamsRoutine(uri, resultCallback));

        /// <summary>
        /// Ask the server for a json.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        private IEnumerator GetJsonWithoutParamsRoutine(string uri, Action<bool, string> resultCallback)
        {
            UnityWebRequest request = new UnityWebRequest(Config.Uri + uri, UnityWebRequest.kHttpVerbGET)
                                      {
                                          downloadHandler = new DownloadHandlerBuffer()
                                      };

            yield return PerformStringOrJsonRequest(request, resultCallback);
        }
        
        /// <summary>
        /// Ask the server for bytes.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        public void GetBytesWithoutParams(string uri, Action<bool, byte[]> resultCallback) =>
            CoroutineRunner.Instance.RunRoutine(GetBytesWithoutParamsRoutine(uri, resultCallback));

        /// <summary>
        /// Ask the server for bytes.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        private IEnumerator GetBytesWithoutParamsRoutine(string uri, Action<bool, byte[]> resultCallback)
        {
            UnityWebRequest request = new UnityWebRequest(Config.Uri + uri, UnityWebRequest.kHttpVerbGET)
                                      {
                                          downloadHandler = new DownloadHandlerBuffer()
                                      };

            yield return PerformByteRequest(request, resultCallback);
        }

        /// <summary>
        /// Ask the server for a json sending some json parameter.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="jsonParam">The parameter to send.</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        public void PostForJsonTextWithJsonParam(string uri, string jsonParam, Action<bool, string> resultCallback) =>
            CoroutineRunner.Instance.RunRoutine(PostForJsonTextWithJsonParamRoutine(uri, jsonParam, resultCallback));

        /// <summary>
        /// Internal coroutine to ask the server for a json sending some json parameter.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="jsonParam">The parameter to send.</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.</param>
        private IEnumerator PostForJsonTextWithJsonParamRoutine(string uri,
                                                                string jsonParam,
                                                                Action<bool, string> resultCallback)
        {
            UnityWebRequest request = new UnityWebRequest(Config.Uri + uri, UnityWebRequest.kHttpVerbPOST)
                                      {
                                          uploadHandler =
                                              new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonParam))
                                              {
                                                  contentType = "application/json"
                                              },
                                          downloadHandler = new DownloadHandlerBuffer()
                                      };

            request.SetRequestHeader("Content-Type", "application/json");

            yield return PerformStringOrJsonRequest(request, resultCallback);
        }

        /// <summary>
        /// Performs the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="resultCallback"></param>
        /// <returns></returns>
        private IEnumerator PerformStringOrJsonRequest(UnityWebRequest request, Action<bool, string> resultCallback)
        {
            yield return request.SendWebRequest();

            string result;
            bool success = !(request.isNetworkError || request.isHttpError);

            if (success)
                result = Encoding.UTF8.GetString(request.downloadHandler.data);
            else
            {
                GetLogger().Error("Web request response: " + request.responseCode + ".");
                GetLogger().Error("Web request error: " + request.error + ".");
                result = request.responseCode.ToString();
            }

            resultCallback.Invoke(success, result);
        }
        
        /// <summary>
        /// Performs the given request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="resultCallback"></param>
        /// <returns></returns>
        private IEnumerator PerformByteRequest(UnityWebRequest request, Action<bool, byte[]> resultCallback)
        {
            yield return request.SendWebRequest();

            byte[] result;
            bool success = !(request.isNetworkError || request.isHttpError);

            if (success)
                result = request.downloadHandler.data;
            else
            {
                GetLogger().Error("Web request response: " + request.responseCode + ".");
                GetLogger().Error("Web request error: " + request.error + ".");
                result = null;
            }

            resultCallback.Invoke(success, result);
        }
    }
}