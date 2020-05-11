using UnityEngine;
using WhateverDevs.Core.Runtime.Serialization;
using Zenject;

namespace WhateverDevs.WebServiceConnector.Runtime.Auth
{
    /// <summary>
    /// Installer for the non auth webservice connector.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/WebServiceConnector/AuthInstaller",
                     fileName = "AuthServiceConnectorInstaller")]
    public class AuthWebServiceConnectorInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// Install the connector to all that ask for the interface.
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<ISerializer<string>>()
                     .To<JsonSerializer>()
                     .AsCached()
                     .WhenInjectedInto<AuthWebServiceConnector>()
                     .Lazy();
            Container.Bind<IAuthWebServiceConnector>().To<AuthWebServiceConnector>().AsSingle().Lazy();
        }
    }
}