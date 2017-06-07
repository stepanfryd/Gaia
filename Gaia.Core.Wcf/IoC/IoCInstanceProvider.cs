using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Gaia.Core.IoC;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity instance provider.
	/// </summary>
	public sealed class IoCInstanceProvider : IInstanceProvider
	{
		#region Constructors

		/// <summary>
		///   Initializes a new instance of the <see cref="IoCInstanceProvider" /> class.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		/// <param name="contractType">
		///   Type of the contract.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   container
		///   or
		///   contractType is null.
		/// </exception>
		public IoCInstanceProvider(IContainer container, Type contractType)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (contractType == null)
			{
				throw new ArgumentNullException(nameof(contractType));
			}

			Contract.EndContractBlock();

			_container = container;
			_contractType = contractType;
		}

		#endregion

		#region Fields

		/// <summary>
		///   The container.
		/// </summary>
		private readonly IContainer _container;

		/// <summary>
		///   The contract type.
		/// </summary>
		private readonly Type _contractType;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///   Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext" /> object.
		/// </summary>
		/// <param name="instanceContext">
		///   The current <see cref="T:System.ServiceModel.InstanceContext" /> object.
		/// </param>
		/// <param name="message">
		///   The message that triggered the creation of a service object.
		/// </param>
		/// <returns>
		///   The service object.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///   instanceContext is null.
		/// </exception>
		public object GetInstance(InstanceContext instanceContext, Message message)
		{
			if (instanceContext == null)
			{
				throw new ArgumentNullException(nameof(instanceContext));
			}

			Contract.EndContractBlock();

			return _container.Resolve(_contractType);
		}

		/// <summary>
		///   Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext" /> object.
		/// </summary>
		/// <param name="instanceContext">
		///   The current <see cref="T:System.ServiceModel.InstanceContext" /> object.
		/// </param>
		/// <returns>
		///   A user-defined service object.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		///   instanceContext is null.
		/// </exception>
		public object GetInstance(InstanceContext instanceContext)
		{
			if (instanceContext == null)
			{
				throw new ArgumentNullException(nameof(instanceContext));
			}

			Contract.EndContractBlock();

			return GetInstance(instanceContext, null);
		}

		/// <summary>
		///   Called when an <see cref="T:System.ServiceModel.InstanceContext" /> object recycles a service object.
		/// </summary>
		/// <param name="instanceContext">
		///   The service's instance context.
		/// </param>
		/// <param name="instance">
		///   The service object to be recycled.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   instanceContext is null.
		/// </exception>
		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
			if (instanceContext == null)
			{
				throw new ArgumentNullException(nameof(instanceContext));
			}

			Contract.EndContractBlock();

			instance = null;
		}

		#endregion
	}
}