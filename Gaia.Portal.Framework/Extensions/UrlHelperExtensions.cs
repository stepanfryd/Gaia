using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gaia.Portal.Framework.Extensions
{
	public static class UrlHelperExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			Tools.GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			return urlHelper.Action(actionName, controllerName, routeValuesDictionary);
		}
	}
}