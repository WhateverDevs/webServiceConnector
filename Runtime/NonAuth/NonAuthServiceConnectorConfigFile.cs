using System;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;

namespace WhateverDevs.WebServiceConnector.Runtime.NonAuth
{
    /// <summary>
    /// Scriptable that stores the configuration for the non auth service connector.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/WebServiceConnector/NonAuthConfig", fileName = "NonAuthServiceConfig")]
    public class
        NonAuthServiceConnectorConfigFile : ConfigurationScriptableHolderUsingFirstValidPersister<
            NonAuthServiceConnectorConfig>
    {
    }

    /// <summary>
    /// Class that stores the configuration data for the non auth service connector.
    /// </summary>
    [Serializable]
    public class NonAuthServiceConnectorConfig : ConfigurationData
    {
        /// <summary>
        /// Uri the connector will use.
        /// </summary>
        public string Uri;
    }
}