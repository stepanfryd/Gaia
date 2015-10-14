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
using System.Web.Mvc;
using Gaia.Core.IoC;
using Gaia.Portal.Framework.Configuration;
using Gaia.Portal.Framework.Configuration.Modules;
using Gaia.Portal.Framework.Security;
using Microsoft.Practices.Unity;

namespace Gaia.Portal.Framework
{
	public abstract class GaiaWebViewPage : WebViewPage
	{
		#region Public members

		public IPermissionManager PermissionManager => Resolve<IPermissionManager>();

		public IWebModuleProvider ModuleProvider => Resolve<IWebModuleProvider>();

		public IConfiguration Configuration => Resolve<IConfiguration>();

		public string NgController { get; set; }

		#endregion

		#region Private and protected

		public override void InitHelpers()
		{
			base.InitHelpers();
			// Bootstrap = new BootstrapHelper(base.ViewContext, this);
		}

		private T Resolve<T>()
		{
			return Container.Instance.Resolve<T>();
		}

		#endregion
	}

	public abstract class GaiaWebViewPage<TModel> : WebViewPage<TModel>
	{
		#region Public members

		public IPermissionManager PermissionManager => Resolve<IPermissionManager>();

		public IWebModuleProvider ModuleProvider => Resolve<IWebModuleProvider>();

		public IConfiguration Configuration => Resolve<IConfiguration>();

		public string NgController { get; set; }

		#endregion

		#region Private and protected

		public override void InitHelpers()
		{
			base.InitHelpers();
			// Bootstrap = new BootstrapHelper<TModel>(base.ViewContext, this);
		}

		private T Resolve<T>()
		{
			return Container.Instance.Resolve<T>();
		}

		#endregion
	}
}