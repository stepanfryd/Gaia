using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Gaia.Portal.Framework.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static void RenderAction<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			Tools.GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			htmlHelper.RenderAction(actionName, controllerName, routeValuesDictionary);
		}


		public static MvcHtmlString Action<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			Tools.GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			return htmlHelper.Action(actionName, controllerName, routeValuesDictionary);
		}
	}
}