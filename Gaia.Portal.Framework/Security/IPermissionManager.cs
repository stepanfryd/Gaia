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
		bool HasAccess(IIdentity identity, IDictionary<string, object> routeValues);

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
		bool HasAccess(IIdentity identity, Route route);

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
		IList<IWebModule> GetAccessibleModules(IIdentity identity);

		/// <summary>
		///   List modules which are accessible to current user
		/// </summary>
		/// <returns></returns>
		IList<IWebModule> GetAccessibleModules(IPrincipal principal);
	}
}