using System.Web.Http;
using System.Web.Http.Controllers;
using Gaia.Portal.Framework.Controllers.Api;

namespace Gaia.Portal.Framework
{
	public class HttpErrorAwareControllerActionSelector : ApiControllerActionSelector
	{
		public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
		{
			HttpActionDescriptor decriptor = null;
			try
			{
				decriptor = base.SelectAction(controllerContext);
			}
			catch (HttpResponseException ex)
			{
				var routeData = controllerContext.RouteData;
				routeData.Values["action"] = "HandleError";
				routeData.Values["statusCode"] = ex.Response.StatusCode;

				IHttpController httpController = new ApiErrorController();
				controllerContext.Controller = httpController;
				controllerContext.ControllerDescriptor = new HttpControllerDescriptor(controllerContext.Configuration, "ApiError",
					httpController.GetType());
				decriptor = base.SelectAction(controllerContext);
			}
			return decriptor;
		}
	}
}