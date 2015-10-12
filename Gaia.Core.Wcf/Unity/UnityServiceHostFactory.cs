// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityServiceHostFactory.cs" company="Rolosoft Ltd">
//   © Rolosoft Ltd
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region License

// Copyright 2014 Rolosoft Ltd
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace Unity.Wcf
{
    #region Usings

    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     The unity service host factory.
    /// </summary>
    public abstract class UnityServiceHostFactory : ServiceHostFactory
    {
        #region Methods

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        protected abstract void ConfigureContainer(IUnityContainer container);

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base
        ///     address.
        /// </summary>
        /// <param name="serviceType">
        /// Specifies the type of service to host.
        /// </param>
        /// <param name="baseAddresses">
        /// The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the
        ///     base addresses for the service hosted.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var container = new UnityContainer();

            this.ConfigureContainer(container);

            return new UnityServiceHost(container, serviceType, baseAddresses);
        }

        #endregion
    }
}