using System.Web.Mvc;
using Gaia.Portal.Framework.Configuration;
using Gaia.Portal.Framework.Security;
using Microsoft.Practices.Unity;

namespace Gaia.Portal.Framework.Web
{
	/// <summary>
	/// Base application controller
	/// </summary>
	public class BaseController : Controller
	{
		[Dependency]
		public IPermissionManager Permissions { get; set; }

		[Dependency]
		public IConfiguration Configuration { get; set; }
	}
}