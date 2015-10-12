using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Gaia.Portal.Framework.Configuration.Modules;
using Microsoft.Practices.Unity;

namespace Gaia.Portal.Framework.Security
{
	public class WindowsPermissionManager : IPermissionManager
	{
		private readonly Permissions _definitions;


		public WindowsPermissionManager(IPermissionsProvider permissionProvider)
		{
			_definitions = permissionProvider.GetPermissions();
		}

		[Dependency]
		public IWebModuleProvider ModuleProvider { get; set; }

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

		public bool HasAccess(string userName, IDictionary<string, object> routeValues)
		{
			Route route;
			if (IsRouteSecure(routeValues, out route))
			{
				using (var identity = new WindowsIdentity(userName))
				{
					return HasAccess(identity, route);
				}
			}

			return true;
		}

		public bool HasAccess(WindowsIdentity identity, IDictionary<string, object> routeValues)
		{
			Route route;
			return !IsRouteSecure(routeValues, out route) || HasAccess(identity, route);
		}

		public bool HasAccess(IPrincipal identity, IDictionary<string, object> routeValues)
		{
			Route route;
			return !IsRouteSecure(routeValues, out route) || HasAccess(identity, route);
		}

		public bool HasAccess(IDictionary<string, object> routeValues)
		{
			Route route;
			return !IsRouteSecure(routeValues, out route) || HasAccess(Thread.CurrentPrincipal, route);
		}

		public bool HasAccess(WindowsIdentity identity, Route route)
		{
			return HasAccess(new WindowsPrincipal(identity), route);
		}

		public bool HasAccess(IPrincipal principal, Route route)
		{
			foreach (var role in _definitions.Roles.Where(r => route.Roles.Contains(r.Name)))
			{
				foreach (var member in role.Members)
				{
					var ml = member;
					var start = ".\\";
					var ind = ml.IndexOf(start, StringComparison.Ordinal);
					if (ind >= 0)
					{
						ml = $"{Environment.MachineName}\\{ml.Substring(ind + start.Length)}";
					}
					if (principal.IsInRole(ml))
					{
						return true;
					}
				}
			}

			return false;
		}

		public IList<IWebModule> GetAccessibleModules()
		{
			return GetAccessibleModules(Thread.CurrentPrincipal);
		}

		public IList<IWebModule> GetAccessibleModules(string userName)
		{
			return GetAccessibleModules(new WindowsIdentity(userName));
		}

		public IList<IWebModule> GetAccessibleModules(WindowsIdentity identity)
		{
			return GetAccessibleModules(new WindowsPrincipal(identity));
		}

		public IList<IWebModule> GetAccessibleModules(IPrincipal principal)
		{
			return ModuleProvider?.Modules.Where(module => !string.IsNullOrEmpty(module?.AreaName))
				.Select(
					module => new {module, rArea = new Dictionary<string, object> {{Constants.RouteValues.Area, module.AreaName}}})
				.Where(@t => HasAccess(principal, @t.rArea))
				.Select(@t => @t.module).ToList();
		}
	}
}