using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common.Logging;
using Gaia.Core.Exceptions;
using Gaia.Core.IoC;

namespace Gaia.Portal.Framework.IoC.Mvc
{
	/// <summary>
	///   An implementation of <see cref="T:System.Web.Mvc.IDependencyResolver" /> that wraps a Unity container.
	/// </summary>
	public class GaiaDependencyResolver : IDependencyResolver
	{
		private readonly IContainer _container;

		/// <summary>
		///   Initializes a new instance of the <see cref="T:Gaia.Portal.Framework.IoC.GaiaDependencyResolver" /> class for a
		///   container.
		/// </summary>
		/// <param name="container">
		///   The <see cref="T:Gaia.Core.IoC.IContainer" /> to wrap with the
		///   <see cref="T:System.Web.Mvc.IDependencyResolver" />
		///   interface implementation.
		/// </param>
		public GaiaDependencyResolver(IContainer container)
		{
			_container = container;
		}

		/// <summary>
		///   Resolves an instance of the default requested type from the container.
		/// </summary>
		/// <param name="serviceType">The <see cref="T:System.Type" /> of the object to get from the container.</param>
		/// <returns>
		///   The requested object.
		/// </returns>
		public object GetService(Type serviceType)
		{
			if (typeof (IController).IsAssignableFrom(serviceType))
			{
				return _container.Resolve(serviceType);
			}
			try
			{
				return _container.Resolve(serviceType);
			}
			catch (GaiaBaseException ex)
			{
				return null;
			}
		}

		/// <summary>
		///   Resolves multiply registered services.
		/// </summary>
		/// <param name="serviceType">The type of the requested services.</param>
		/// <returns>
		///   The requested services.
		/// </returns>
		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _container.ResolveAll(serviceType);
		}
	}
}