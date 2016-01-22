using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Common.Logging;
using Gaia.Core.IoC;

namespace Gaia.Portal.Framework.IoC.WebApi
{
	/// <summary>
	///   An implementation of the <see cref="T:System.Web.Http.Dependencies.IDependencyResolver" /> interface that wraps a
	///   Unity container.
	/// </summary>
	public sealed class GaiaWebApiDependencyResolver : IDependencyResolver
	{
		private readonly IContainer _container;
		private readonly SharedDependencyScope _sharedScope;
		private readonly ILog _log;

		/// <summary>
		///   Initializes a new instance of the <see cref="T:Gaia.Portal.Framework.IoC.WebApi.GaiaWebApiDependencyResolver" /> class for
		///   a container.
		/// </summary>
		/// <param name="container">
		///   The <see cref="T:Gaia.Core.IoC.IContainer" /> to wrap with the
		///   <see cref="T:System.Web.Http.Dependencies.IDependencyResolver" />
		///   interface implementation.
		/// </param>
		public GaiaWebApiDependencyResolver(IContainer container)
		{
			_log = LogManager.GetLogger(GetType());
			if (container == null)
				throw new ArgumentNullException(nameof(container));
			_container = container;
			_sharedScope = new SharedDependencyScope(container);
		}

		/// <summary>
		///   Reuses the same scope to resolve all the instances.
		/// </summary>
		/// <returns>
		///   The shared dependency scope.
		/// </returns>
		public IDependencyScope BeginScope()
		{
			return _sharedScope;
		}

		/// <summary>
		///   Disposes the wrapped <see cref="T:Gaia.Core.IoC.IContainer" />.
		/// </summary>
		public void Dispose()
		{
			_container.Dispose();
			_sharedScope.Dispose();
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
			try
			{
				return _container.Resolve(serviceType);
			}
			catch (Exception ex)
			{
				_log.Error($"Cannot resolve service type ${serviceType}", ex);
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
			try
			{
				return _container.ResolveAll(serviceType);
			}
			catch (Exception ex)
			{
				_log.Error($"Cannot resolve service type ${serviceType}", ex);
				return null;
			}
		}

		private sealed class SharedDependencyScope : IDependencyScope
		{
			private readonly IContainer _container;

			public SharedDependencyScope(IContainer container)
			{
				_container = container;
			}

			public object GetService(Type serviceType)
			{
				return _container.Resolve(serviceType);
			}

			public IEnumerable<object> GetServices(Type serviceType)
			{
				return _container.ResolveAll(serviceType);
			}

			public void Dispose()
			{
			}
		}
	}
}