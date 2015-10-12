// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityContractBehavior.cs" company="Rolosoft Ltd">
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
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    /// <summary>
    ///     The unity contract behavior.
    /// </summary>
    public sealed class UnityContractBehavior : IContractBehavior
    {
        #region Fields

        /// <summary>
        ///     The instance provider.
        /// </summary>
        private readonly IInstanceProvider instanceProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityContractBehavior"/> class.
        /// </summary>
        /// <param name="instanceProvider">
        /// The instance provider.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// instanceProvider is null.
        /// </exception>
        public UnityContractBehavior(IInstanceProvider instanceProvider)
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException("instanceProvider");
            }

            Contract.EndContractBlock();

            this.instanceProvider = instanceProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description to modify.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint to modify.
        /// </param>
        /// <param name="bindingParameters">
        /// The objects that binding elements require to support the behavior.
        /// </param>
        public void AddBindingParameters(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description for which the extension is intended.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint.
        /// </param>
        /// <param name="clientRuntime">
        /// The client runtime.
        /// </param>
        public void ApplyClientBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract description to be modified.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint that exposes the contract.
        /// </param>
        /// <param name="dispatchRuntime">
        /// The dispatch runtime that controls service execution.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// dispatchRuntime is null.
        /// </exception>
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, 
            ServiceEndpoint endpoint, 
            DispatchRuntime dispatchRuntime)
        {
            if (dispatchRuntime == null)
            {
                throw new ArgumentNullException("dispatchRuntime");
            }

            Contract.EndContractBlock();

            dispatchRuntime.InstanceProvider = this.instanceProvider;
            dispatchRuntime.InstanceContextInitializers.Add(new UnityInstanceContextInitializer());
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">
        /// The contract to validate.
        /// </param>
        /// <param name="endpoint">
        /// The endpoint to validate.
        /// </param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}