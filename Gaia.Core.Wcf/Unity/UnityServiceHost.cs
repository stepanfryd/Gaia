// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityServiceHost.cs" company="Rolosoft Ltd">
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
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     The unity service host.
    /// </summary>
    public sealed class UnityServiceHost : ServiceHost
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHost"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="serviceType">
        /// Type of the service.
        /// </param>
        /// <param name="baseAddresses">
        /// The base addresses.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// container is null.
        /// </exception>
        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Contract.EndContractBlock();

            this.ApplyServiceBehaviors(container);

            this.ApplyContractBehaviors(container);

            foreach (ContractDescription contractDescription in this.ImplementedContracts.Values)
            {
                var contractBehavior =
                    new UnityContractBehavior(new UnityInstanceProvider(container, contractDescription.ContractType));

                contractDescription.Behaviors.Add(contractBehavior);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the contract behaviors.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        private void ApplyContractBehaviors(IUnityContainer container)
        {
            var registeredContractBehaviors = container.ResolveAll<IContractBehavior>();

            foreach (IContractBehavior contractBehavior in registeredContractBehaviors)
            {
                foreach (ContractDescription contractDescription in this.ImplementedContracts.Values)
                {
                    contractDescription.Behaviors.Add(contractBehavior);
                }
            }
        }

        /// <summary>
        /// Applies the service behaviors.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        private void ApplyServiceBehaviors(IUnityContainer container)
        {
            var registeredServiceBehaviors = container.ResolveAll<IServiceBehavior>();

            foreach (IServiceBehavior serviceBehavior in registeredServiceBehaviors)
            {
                this.Description.Behaviors.Add(serviceBehavior);
            }
        }

        #endregion
    }
}