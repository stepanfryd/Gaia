using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Gaia.Portal.Framework.Validation;

namespace Gaia.Portal.Framework.Web.Filters
{
	/// <summary>
	///   Attribute describes if action method is automaticaly validated during filter
	/// </summary>
	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var method = actionContext.ActionDescriptor as ReflectedHttpActionDescriptor;

			if (method?.MethodInfo != null)
			{
				ValidationHelpers.ClearIgnoredProperties(method.MethodInfo, actionContext.ModelState);
			}

			// if model is not valid response standard error way
			if (actionContext.ModelState.IsValid == false)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(
					HttpStatusCode.BadRequest, actionContext.ModelState);
			}
		}
	}
}