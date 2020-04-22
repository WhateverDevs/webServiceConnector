using UnityEngine;
using Zenject;

namespace WhateverDevs.WebServiceConnector.Runtime.NonAuth
{
    /// <summary>
    /// Installer for the non auth webservice connector.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/WebServiceConnector/NonAuthInstaller",
                     fileName = "NonAuthServiceConnectorInstaller")]
    public class NonAuthServiceConnectorInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// Install the connector to all that ask for the interface.
        /// </summary>
        public override void InstallBindings() =>
            Container.Bind<INonAuthWebServiceConnector>().To<NonAuthServiceConnector>().AsSingle().Lazy();
    }
}