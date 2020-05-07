using System;
using WhateverDevs.WebServiceConnector.Runtime.NonAuth;

namespace WhateverDevs.WebServiceConnector.Runtime.Auth
{
    /// <summary>
    /// Interface that defines what a WebServiceConnector with authentication should do.
    /// </summary>
    public interface IAuthWebServiceConnector
    {
        /// <summary>
        /// Ask the server for a json sending some json parameter.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        void PostForJsonTextWithoutParams(string uri, Action<bool, string> resultCallback);
    }
}