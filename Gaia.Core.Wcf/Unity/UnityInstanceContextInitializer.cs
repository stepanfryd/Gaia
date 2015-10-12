// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityInstanceContextInitializer.cs" company="Rolosoft Ltd">
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

    #endregion

    /// <summary>
    ///     The unity instance context initializer.
    /// </summary>
    public sealed class UnityInstanceContextInitializer : IInstanceContextInitializer
    {
        #region Public Methods and Operators

        /// <summary>
        /// Provides the ability to modify the newly created <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">
        /// The system-supplied instance context.
        /// </param>
        /// <param name="message">
        /// The message that triggered the creation of the instance context.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// instanceContext is null.
        /// </exception>
        public void Initialize(InstanceContext instanceContext, Message message)
        {
            if (instanceContext == null)
            {
                throw new ArgumentNullException("instanceContext");
            }

            Contract.EndContractBlock();

            instanceContext.Extensions.Add(new UnityInstanceContextExtension());
        }

        #endregion
    }
}