using System.Web.Mvc;
using Gaia.Portal.Framework.Validation;

namespace Gaia.Portal.Framework.Web.Filters
{
	public class ModelStateClearAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var method = filterContext.ActionDescriptor as ReflectedActionDescriptor;

			if (method?.MethodInfo != null)
			{
				ValidationHelpers.ClearIgnoredProperties(method.MethodInfo, filterContext.Controller.ViewData.ModelState);
			}

			base.OnActionExecuting(filterContext);
		}
	}
}