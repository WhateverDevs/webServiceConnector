using System;

namespace WhateverDevs.WebServiceConnector.Runtime.NonAuth
{
    /// <summary>
    /// Interface that defines what a WebServiceConnector without authentication should do.
    /// </summary>
    public interface INonAuthWebServiceConnector
    {
        /// <summary>
        /// Ask the server for a json sending some json parameter.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="jsonParam">The parameter to send.</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        void PostForJsonTextWithJsonParam(string uri, string jsonParam, Action<bool, string> resultCallback);
    }
}