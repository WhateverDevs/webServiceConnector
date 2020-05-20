using System;

namespace WhateverDevs.WebServiceConnector.Runtime.Auth
{
    /// <summary>
    /// Interface that defines what a WebServiceConnector with authentication should do.
    /// </summary>
    public interface IAuthWebServiceConnector
    {
        /// <summary>
        /// Are we logged in?
        /// </summary>
        bool LoggedIn();
        
        /// <summary>
        /// Login to the service.
        /// </summary>
        /// <param name="email">Login credentials.</param>
        /// <param name="password">Login credentials.</param>
        /// <param name="resultCallback">Login result.</param>
        void Login(string email, string password, Action<LoginResult> resultCallback);

        /// <summary>
        /// Logs out.
        /// </summary>
        void LogOut();

        /// <summary>
        /// Ask the server for a json without params.
        /// </summary>
        /// <param name="uri">Relative uri inside the service starting with "/".</param>
        /// <param name="resultCallback">Method that will be called when the request is finished.
        /// The bool parameters shows if it was successful, the string will be the result if successful, the error if not.</param>
        void GetJsonTextWithoutParams(string uri, Action<bool, string> resultCallback);
        
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