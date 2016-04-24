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
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Gaia.Portal.Framework.Extensions
{
	/// <summary>
	/// Class contains html helpers extensions
	/// </summary>
	public static class HelperExtensions
	{
		/// <summary>
		/// Render actin using expression
		/// </summary>
		/// <typeparam name="TController"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expr"></param>
		/// <param name="areaName"></param>
		/// <param name="routeValues"></param>
		public static void RenderAction<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			htmlHelper.RenderAction(actionName, controllerName, routeValuesDictionary);
		}

		/// <summary>
		/// Html action from expression
		/// </summary>
		/// <typeparam name="TController"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expr"></param>
		/// <param name="areaName"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public static MvcHtmlString Action<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			return htmlHelper.Action(actionName, controllerName, routeValuesDictionary);
		}

		/// <summary>
		/// Helper method for html actions
		/// </summary>
		/// <typeparam name="TController"></typeparam>
		/// <param name="expr"></param>
		/// <param name="action"></param>
		/// <param name="controller"></param>
		/// <param name="dict"></param>
		/// <param name="areaName"></param>
		/// <param name="routeValues"></param>
		public static void GetExpressionData<TController>(Expression<Action<TController>> expr, out string action,
			out string controller, out RouteValueDictionary dict, string areaName = null, object routeValues = null)
		{
			controller = typeof(TController).Name.Replace("Controller", "");
			action = ((MethodCallExpression)expr.Body).Method.Name;

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

		/// <summary>
		/// Action from url helper
		/// </summary>
		/// <typeparam name="TController"></typeparam>
		/// <param name="urlHelper"></param>
		/// <param name="expr"></param>
		/// <param name="areaName"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Action<TController>> expr,
			string areaName = null, object routeValues = null) where TController : Controller
		{
			string actionName;
			string controllerName;
			RouteValueDictionary routeValuesDictionary;
			GetExpressionData(expr, out actionName, out controllerName, out routeValuesDictionary, areaName, routeValues);

			return urlHelper.Action(actionName, controllerName, routeValuesDictionary);
		}
	}
}