using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace Gaia.Portal.Framework.Security
{
	public class GaiaAuthorizeAttribute : AuthorizeAttribute
	{
		[Dependency]
		public IPermissionManager PermissionManager { get; set; }

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var hasAccess = PermissionManager.HasAccess(filterContext.RouteData.Values);
			if (!hasAccess)
			{
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
						{"Controller", "Home"},
						{"Action", "Error"},
						{"statusCode", 401}
					});
			}
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			throw new HttpResponseException(HttpStatusCode.Unauthorized);
		}
	}
}