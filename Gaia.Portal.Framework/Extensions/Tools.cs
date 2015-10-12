using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gaia.Portal.Framework.Extensions
{
	public static class Tools
	{
		public static void GetExpressionData<TController>(Expression<Action<TController>> expr, out string action,
			out string controller, out RouteValueDictionary dict, string areaName = null, object routeValues = null)
		{
			controller = typeof (TController).Name.Replace("Controller", "");
			action = ((MethodCallExpression) expr.Body).Method.Name;

			dict = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
			if (dict.ContainsKey("area"))
			{
				dict["area"] = areaName;
			}
			else
			{
				dict.Add("area", areaName);
			}
		}
	}
}