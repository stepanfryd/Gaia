using System.Collections.Generic;
using System.Security.Principal;
using Gaia.Portal.Framework.Configuration.Modules;

namespace Gaia.Portal.Framework.Security
{
	/// <summary>
	///   Permission manager provides authorization functionality
	/// </summary>
	public interface IPermissionManager
	{
		/// <summary>
		///   Method validates if user has an access to provided route
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		bool HasAccess(string userName, IDictionary<string, object> routeValues);

		/// <summary>
		///   Method validates if user has an access to provided route
		/// </summary>
		/// <param name="identity"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		bool HasAccess(WindowsIdentity identity, IDictionary<string, object> routeValues);

		/// <summary>
		///   Method validates if provided route is declared as secured
		/// </summary>
		/// <param name="routeValues"></param>
		/// <param name="securedRoute"></param>
		/// <returns></returns>
		bool IsRouteSecure(IDictionary<string, object> routeValues, out Route securedRoute);

		/// <summary>
		///   Method validates if provided route is declared as secured
		/// </summary>
		/// <param name="identity"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		bool HasAccess(IPrincipal identity, IDictionary<string, object> routeValues);

		/// <summary>
		///   Method validates if provided route is declared as secured
		/// </summary>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		bool HasAccess(IDictionary<string, object> routeValues);

		/// <summary>
		///   Method validates if provided route is declared as secured
		/// </summary>
		/// <param name="identity"></param>
		/// <param name="route"></param>
		/// <returns></returns>
		bool HasAccess(WindowsIdentity identity, Route route);

		/// <summary>
		///   Method validates if provided route is declared as secured
		/// </summary>
		/// <param name="principal"></param>
		/// <param name="route"></param>
		/// <returns></returns>
		bool HasAccess(IPrincipal principal, Route route);

		/// <summary>
		///   List modules which are accessible to current user
		/// </summary>
		/// <returns></returns>
		IList<IWebModule> GetAccessibleModules();

		/// <summary>
		///   List modules which are accessible to current user
		/// </summary>
		/// <returns></returns>
		IList<IWebModule> GetAccessibleModules(string userName);

		/// <summary>
		///   List modules which are accessible to current user
		/// </summary>
		/// <returns></returns>
		IList<IWebModule> GetAccessibleModules(WindowsIdentity identity);

		/// <summary>
		///   List modules which are accessible to current user
		/// </summary>
		/// <returns></returns>
		IList<IWebModule> GetAccessibleModules(IPrincipal principal);
	}
}