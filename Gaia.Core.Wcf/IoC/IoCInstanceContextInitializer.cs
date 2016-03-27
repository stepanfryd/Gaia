using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity instance context initializer.
	/// </summary>
	public sealed class IoCInstanceContextInitializer : IInstanceContextInitializer
	{
		#region Interface Implementations

		#region Public Methods and Operators

		/// <summary>
		///   Provides the ability to modify the newly created <see cref="T:System.ServiceModel.InstanceContext" /> object.
		/// </summary>
		/// <param name="instanceContext">
		///   The system-supplied instance context.
		/// </param>
		/// <param name="message">
		///   The message that triggered the creation of the instance context.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   instanceContext is null.
		/// </exception>
		public void Initialize(InstanceContext instanceContext, Message message)
		{
			if (instanceContext == null)
			{
				throw new ArgumentNullException(nameof(instanceContext));
			}

			Contract.EndContractBlock();

			instanceContext.Extensions.Add(new IoCInstanceContextExtension());
		}

		#endregion

		#endregion
	}
}