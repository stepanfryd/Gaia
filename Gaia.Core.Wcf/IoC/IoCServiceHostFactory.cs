using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Gaia.Core.IoC;

namespace Gaia.Core.Wcf.IoC
{
	/// <summary>
	///   The unity service host factory.
	/// </summary>
	public abstract class IoCServiceHostFactory : ServiceHostFactory
	{
		#region Methods

		/// <summary>
		///   Configures the container.
		/// </summary>
		/// <param name="container">
		///   The container.
		/// </param>
		protected abstract void ConfigureContainer(IContainer container);

		/// <summary>
		///   Creates a <see cref="T:System.ServiceModel.ServiceHost" /> for a specified type of service with a specific base
		///   address.
		/// </summary>
		/// <param name="serviceType">
		///   Specifies the type of service to host.
		/// </param>
		/// <param name="baseAddresses">
		///   The <see cref="T:System.Array" /> of type <see cref="T:System.Uri" /> that contains the
		///   base addresses for the service hosted.
		/// </param>
		/// <returns>
		///   A <see cref="T:System.ServiceModel.ServiceHost" /> for the type of service specified with a specific base address.
		/// </returns>
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			var container = Container.Instance;
			ConfigureContainer(container);

			return new IoCServiceHost(container, serviceType, baseAddresses);
		}

		#endregion
	}
}