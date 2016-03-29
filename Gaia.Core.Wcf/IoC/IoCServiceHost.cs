using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Description;
using Gaia.Core.IoC;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity service host.
	/// </summary>
	public sealed class IoCServiceHost : ServiceHost
	{
		#region Constructors

		/// <summary>
		///   Initializes a new instance of the <see cref="IoCServiceHost" /> class.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		/// <param name="serviceType">
		///   Type of the service.
		/// </param>
		/// <param name="baseAddresses">
		///   The base addresses.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///   container is null.
		/// </exception>
		public IoCServiceHost(IContainer container, Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			Contract.EndContractBlock();

			ApplyServiceBehaviors(container);

			ApplyContractBehaviors(container);

			foreach (var contractDescription in ImplementedContracts.Values)
			{
				var contractBehavior =
					new IoCContractBehavior(new IoCInstanceProvider(container, contractDescription.ContractType));

				contractDescription.Behaviors.Add(contractBehavior);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///   Applies the contract behaviors.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		private void ApplyContractBehaviors(IContainer container)
		{
			var registeredContractBehaviors = container.ResolveAll<IContractBehavior>();

			foreach (var contractBehavior in registeredContractBehaviors)
			{
				foreach (var contractDescription in ImplementedContracts.Values)
				{
					contractDescription.Behaviors.Add(contractBehavior);
				}
			}
		}

		/// <summary>
		///   Applies the service behaviors.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		private void ApplyServiceBehaviors(IContainer container)
		{
			var registeredServiceBehaviors = container.ResolveAll<IServiceBehavior>();

			foreach (var serviceBehavior in registeredServiceBehaviors)
			{
				Description.Behaviors.Add(serviceBehavior);
			}
		}

		#endregion
	}
}