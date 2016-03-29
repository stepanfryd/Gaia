using System;
using System.Diagnostics.Contracts;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity contract behavior.
	/// </summary>
	public sealed class IoCContractBehavior : IContractBehavior
	{
		#region Fields and constants

		#region Fields

		/// <summary>
		///   The instance provider.
		/// </summary>
		private readonly IInstanceProvider _instanceProvider;

		#endregion

		#endregion

		#region Constructors

		/// <summary>
		///   Initializes a new instance of the <see cref="IoCContractBehavior" /> class.
		/// </summary>
		/// <param name="instanceProvider">
		///   The instance provider.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   instanceProvider is null.
		/// </exception>
		public IoCContractBehavior(IInstanceProvider instanceProvider)
		{
			if (instanceProvider == null)
			{
				throw new ArgumentNullException(nameof(instanceProvider));
			}

			Contract.EndContractBlock();

			_instanceProvider = instanceProvider;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///   Configures any binding elements to support the contract behavior.
		/// </summary>
		/// <param name="contractDescription">
		///   The contract description to modify.
		/// </param>
		/// <param name="endpoint">
		///   The endpoint to modify.
		/// </param>
		/// <param name="bindingParameters">
		///   The objects that binding elements require to support the behavior.
		/// </param>
		public void AddBindingParameters(
			ContractDescription contractDescription,
			ServiceEndpoint endpoint,
			BindingParameterCollection bindingParameters)
		{
		}

		/// <summary>
		///   Implements a modification or extension of the client across a contract.
		/// </summary>
		/// <param name="contractDescription">
		///   The contract description for which the extension is intended.
		/// </param>
		/// <param name="endpoint">
		///   The endpoint.
		/// </param>
		/// <param name="clientRuntime">
		///   The client runtime.
		/// </param>
		public void ApplyClientBehavior(
			ContractDescription contractDescription,
			ServiceEndpoint endpoint,
			ClientRuntime clientRuntime)
		{
		}

		/// <summary>
		///   Implements a modification or extension of the client across a contract.
		/// </summary>
		/// <param name="contractDescription">
		///   The contract description to be modified.
		/// </param>
		/// <param name="endpoint">
		///   The endpoint that exposes the contract.
		/// </param>
		/// <param name="dispatchRuntime">
		///   The dispatch runtime that controls service execution.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   dispatchRuntime is null.
		/// </exception>
		public void ApplyDispatchBehavior(
			ContractDescription contractDescription,
			ServiceEndpoint endpoint,
			DispatchRuntime dispatchRuntime)
		{
			if (dispatchRuntime == null)
			{
				throw new ArgumentNullException(nameof(dispatchRuntime));
			}

			Contract.EndContractBlock();

			dispatchRuntime.InstanceProvider = _instanceProvider;
			dispatchRuntime.InstanceContextInitializers.Add(new IoCInstanceContextInitializer());
		}

		/// <summary>
		///   Implement to confirm that the contract and endpoint can support the contract behavior.
		/// </summary>
		/// <param name="contractDescription">
		///   The contract to validate.
		/// </param>
		/// <param name="endpoint">
		///   The endpoint to validate.
		/// </param>
		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}

		#endregion
	}
}