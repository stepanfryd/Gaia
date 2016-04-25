/*
The MIT License (MIT)

Copyright (c) 2012 Stepan Fryd (stepan.fryd@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Gaia.Core.IoC;
using Gaia.Portal.Framework.Configuration.Modules;

namespace Gaia.Portal.Framework.Security
{
	/// <summary>
	/// Base class for permission manager
	/// </summary>
	public abstract class PermissionManagerBase : IPermissionManager
	{
		#region Fields and constants

		private readonly Permissions _definitions;

		#endregion

		#region Public members
		/// <summary>
		/// Web moduler provider
		/// </summary>
		public IWebModuleProvider ModuleProvider
			=> new Lazy<IWebModuleProvider>(() => Container.Instance.Resolve<IWebModuleProvider>()).Value;

		#endregion

		#region Constructors

		protected PermissionManagerBase(IPermissionsProvider permissionProvider)
		{
			_definitions = permissionProvider.GetPermissions();
		}

		#endregion

		#region Interface Implementations
		/// <summary>
		/// Determines if provided route is secure
		/// </summary>
		/// <param name="routeValues">Dictionary contains route values provided by environment</param>
		/// <param name="securedRoute">Returns Gaia route if secured</param>
		/// <returns></returns>
		public bool IsRouteSecure(IDictionary<string, object> routeValues, out Route securedRoute)
		{
			if (routeValues == null)
				throw new ArgumentNullException(nameof(routeValues));

			var routeKeys = routeValues.Keys.ToList();

			foreach (var route in _definitions.Routes.ToList())
			{
				var keysToCompare = routeKeys.Intersect(route.RouteValues.Keys).ToList();
				var trues =
					keysToCompare.Count(ktc => route.RouteValues[ktc].Contains(routeValues[ktc].ToString().ToLowerInvariant()));
				if (trues == keysToCompare.Count)
				{
					securedRoute = route;
					return true;
				}
			}

			securedRoute = null;
			return false;
		}

		/// <summary>
		/// Check whether identity has access to route by route values
		/// </summary>
		/// <param name="identity"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public bool HasAccess(IPrincipal identity, IDictionary<string, object> routeValues)
		{
			Route route;
			return !IsRouteSecure(routeValues, out route) || HasAccess(identity, route);
		}

		/// <summary>
		/// Check whether default thread principal has access to route by route values
		/// </summary>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public bool HasAccess(IDictionary<string, object> routeValues)
		{
			Route route;
			return !IsRouteSecure(routeValues, out route) || HasAccess(Thread.CurrentPrincipal, route);
		}

		/// <summary>
		/// Check whether default thread principal has access to route by route values
		/// </summary>
		/// <param name="principal"></param>
		/// <param name="route"></param>
		/// <returns></returns>
		public abstract bool HasAccess(IPrincipal principal, Route route);

		/// <summary>
		/// Get list of accessible modules
		/// </summary>
		/// <returns></returns>
		public abstract IList<IWebModule> GetAccessibleModules();

		/// <summary>
		/// Het accessible modules for provided principal
		/// </summary>
		/// <param name="principal"></param>
		/// <returns></returns>
		public IList<IWebModule> GetAccessibleModules(IPrincipal principal)
		{
			var modules = ModuleProvider?.Modules.Where(module => !string.IsNullOrEmpty(module?.AreaName))
				.Select(
					m => new { Module = m, rArea = new Dictionary<string, object> {{Constants.RouteValues.Area, m.AreaName}}});

			return modules.Where(t => HasAccess(principal, t.rArea))
				.Select(t => t.Module).ToList();
		}

		#endregion
	}
}