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
using Gaia.Portal.Framework.Configuration.Modules;

namespace Gaia.Portal.Framework.Security
{
	public class WindowsPermissionManager : PermissionManagerBase
	{
		#region Fields and constants

		private readonly Permissions _definitions;

		#endregion

		#region Constructors

		public WindowsPermissionManager(IPermissionsProvider permissionProvider) : base(permissionProvider)
		{
			_definitions = permissionProvider.GetPermissions();
		}

		#endregion

		#region Private and protected

		public override IList<IWebModule> GetAccessibleModules()
		{
			return GetAccessibleModules(Thread.CurrentPrincipal);
		}

		public override bool HasAccess(IPrincipal principal, Route route)
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

		#endregion
	}
}