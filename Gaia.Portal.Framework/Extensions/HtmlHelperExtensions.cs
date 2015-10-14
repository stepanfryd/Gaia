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