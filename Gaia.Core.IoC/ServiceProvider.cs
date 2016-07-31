using System;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// </summary>
	public class ServiceProvider : IServiceProvider
	{
		#region Fields and constants

		private readonly IContainer _container;

		#endregion

		#region Constructors

		/// <summary>
		/// </summary>
		/// <param name="container"></param>
		public ServiceProvider(IContainer container)
		{
			_container = container;
		}

		#endregion

		#region Interface Implementations

		/// <summary>Gets the service object of the specified type.</summary>
		/// <returns>
		///   A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type
		///   <paramref name="serviceType" />.
		/// </returns>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <filterpriority>2</filterpriority>
		public object GetService(Type serviceType)
		{
			return _container.Resolve(serviceType);
		}

		#endregion
	}
}