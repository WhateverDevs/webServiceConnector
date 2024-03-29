﻿using System;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;

namespace WhateverDevs.WebServiceConnector.Runtime.Auth
{
    /// <summary>
    /// Scriptable that stores the configuration for the non auth service connector.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/WebServiceConnector/AuthConfig", fileName = "AuthServiceConfig")]
    public class
        AuthWebServiceConnectorConfigFile : ConfigurationScriptableHolderUsingFirstValidPersister<
            AuthWebServiceConnectorConfig>
    {
    }

    /// <summary>
    /// Class that stores the configuration data for the non auth service connector.
    /// </summary>
    [Serializable]
    public class AuthWebServiceConnectorConfig : ConfigurationData
    {
        /// <summary>
        /// Uri the connector will use.
        /// </summary>
        public string Uri;

        /// <summary>
        /// Uri to login to the service.
        /// </summary>
        public string LoginUri;

        /// <summary>
        /// Uri to logout from the service.
        /// </summary>
        public string LogoutUri;

        /// <summary>
        /// Uri to register to the service.
        /// </summary>
        public string RegisterUri;

        /// <summary>
        /// Clone this data into a new instance of the same type.
        /// </summary>
        /// <typeparam name="TConfigurationData">Type of the configuration.</typeparam>
        /// <returns>The cloned config.</returns>
        protected override TConfigurationData Clone<TConfigurationData>() =>
            new AuthWebServiceConnectorConfig
            {
                Uri = Uri,
                LoginUri = LoginUri,
                LogoutUri = LogoutUri,
                RegisterUri = RegisterUri
            } as TConfigurationData;
    }
}