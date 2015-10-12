// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityInstanceProvider.cs" company="Rolosoft Ltd">
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
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     The unity instance provider.
    /// </summary>
    public sealed class UnityInstanceProvider : IInstanceProvider
    {
        #region Fields

        /// <summary>
        ///     The container.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        ///     The contract type.
        /// </summary>
        private readonly Type contractType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="contractType">
        /// Type of the contract.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// container
        ///     or
        ///     contractType is null.
        /// </exception>
        public UnityInstanceProvider(IUnityContainer container, Type contractType)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (contractType == null)
            {
                throw new ArgumentNullException("contractType");
            }

            Contract.EndContractBlock();

            this.container = container;
            this.contractType = contractType;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">
        /// The current <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </param>
        /// <param name="message">
        /// The message that triggered the creation of a service object.
        /// </param>
        /// <returns>
        /// The service object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// instanceContext is null.
        /// </exception>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            if (instanceContext == null)
            {
                throw new ArgumentNullException("instanceContext");
            }

            Contract.EndContractBlock();

            var childContainer =
                instanceContext.Extensions.Find<UnityInstanceContextExtension>().GetChildContainer(this.container);

            return childContainer.Resolve(this.contractType);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">
        /// The current <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </param>
        /// <returns>
        /// A user-defined service object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// instanceContext is null.
        /// </exception>
        public object GetInstance(InstanceContext instanceContext)
        {
            if (instanceContext == null)
            {
                throw new ArgumentNullException("instanceContext");
            }

            Contract.EndContractBlock();

            return this.GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">
        /// The service's instance context.
        /// </param>
        /// <param name="instance">
        /// The service object to be recycled.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// instanceContext is null.
        /// </exception>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instanceContext == null)
            {
                throw new ArgumentNullException("instanceContext");
            }

            Contract.EndContractBlock();

            instanceContext.Extensions.Find<UnityInstanceContextExtension>().DisposeOfChildContainer();
        }

        #endregion
    }
}