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