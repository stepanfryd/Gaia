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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Gaia.Portal.Framework
{
	public class NamespaceHttpControllerSelector : DefaultHttpControllerSelector
	{
		private const string NAMESPACE_KEY = "namespace";
		private const string CONTROLLER_KEY = "controller";

		private readonly HttpConfiguration _configuration;
		private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;
		private readonly HashSet<string> _duplicates;

		public NamespaceHttpControllerSelector() : base(GlobalConfiguration.Configuration) { }

		public NamespaceHttpControllerSelector(HttpConfiguration config): this()
		{
			_configuration = config;
			_duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
		}

		public new HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			HttpControllerDescriptor decriptor = null;
			try
			{
				decriptor = base.SelectController(request);
			}
			catch (HttpResponseException ex)
			{
				var routeValues = request.GetRouteData().Values;
				routeValues["controller"] = "ApiError";
				routeValues["action"] = "HandleError";
				routeValues["statusCode"] = ex.Response.StatusCode;
				decriptor = base.SelectController(request);
			}
			

			var routeData = request.GetRouteData();
			if (routeData == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			// Get the namespace and controller variables from the route data.
			var namespaceName = GetRouteVariable<string>(routeData, NAMESPACE_KEY);
			if (namespaceName == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			var controllerName = GetRouteVariable<string>(routeData, CONTROLLER_KEY);
			if (controllerName == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			// Find a matching controller.
			var key = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, controllerName);

			HttpControllerDescriptor controllerDescriptor;
			if (_controllers.Value.TryGetValue(key, out controllerDescriptor))
			{
				return controllerDescriptor;
			}
			if (_duplicates.Contains(key))
			{
				throw new HttpResponseException(
					request.CreateErrorResponse(HttpStatusCode.InternalServerError,
						"Multiple controllers were found that match this request."));
			}
			throw new HttpResponseException(HttpStatusCode.NotFound);
		}

		public new IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
		{
			return _controllers.Value;
		}

		private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
		{
			var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

			// Create a lookup table where key is "namespace.controller". The value of "namespace" is the last
			// segment of the full namespace. For example:
			// MyApplication.Controllers.V1.ProductsController => "V1.Products"
			var assembliesResolver = _configuration.Services.GetAssembliesResolver();
			var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

			var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

			foreach (var t in controllerTypes)
			{
				if (t.Namespace != null) {
					var segments = t.Namespace.Split(Type.Delimiter);

					// For the dictionary key, strip "Controller" from the end of the type name.
					// This matches the behavior of DefaultHttpControllerSelector.
					var controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

					var key = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", segments[segments.Length - 1], controllerName);

					// Check for duplicate keys.
					if (dictionary.Keys.Contains(key))
					{
						_duplicates.Add(key);
					}
					else
					{
						dictionary[key] = new HttpControllerDescriptor(_configuration, t.Name, t);
					}
				}
			}

			// Remove any duplicates from the dictionary, because these create ambiguous matches. 
			// For example, "Foo.V1.ProductsController" and "Bar.V1.ProductsController" both map to "v1.products".
			foreach (var s in _duplicates)
			{
				dictionary.Remove(s);
			}
			return dictionary;
		}

		// Get a value from the route data, if present.
		private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
		{
			object result;
			if (routeData.Values.TryGetValue(name, out result))
			{
				return (T) result;
			}
			return default(T);
		}
	}
}