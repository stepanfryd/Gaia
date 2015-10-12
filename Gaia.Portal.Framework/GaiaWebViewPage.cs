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