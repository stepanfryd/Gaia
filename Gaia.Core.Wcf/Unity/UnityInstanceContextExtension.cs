// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityInstanceContextExtension.cs" company="Rolosoft Ltd">
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

    using Microsoft.Practices.Unity;

    #endregion

    /// <summary>
    ///     The unity instance context extension.
    /// </summary>
    public sealed class UnityInstanceContextExtension : IExtension<InstanceContext>
    {
        #region Fields

        /// <summary>
        ///     The child container.
        /// </summary>
        private IUnityContainer childContainer;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Attaches the specified owner.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public void Attach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Detaches the specified owner.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public void Detach(InstanceContext owner)
        {
        }

        /// <summary>
        ///     Disposes the of child container.
        /// </summary>
        public void DisposeOfChildContainer()
        {
            if (this.childContainer != null)
            {
                this.childContainer.Dispose();
            }
        }

        /// <summary>
        /// Gets the child container.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="IUnityContainer"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// container is null.
        /// </exception>
        public IUnityContainer GetChildContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Contract.EndContractBlock();

            return this.childContainer ?? (this.childContainer = container.CreateChildContainer());
        }

        #endregion
    }
}