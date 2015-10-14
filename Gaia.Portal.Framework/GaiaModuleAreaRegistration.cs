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
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using Gaia.Portal.Framework.Configuration.Modules;
using Gaia.Portal.Framework.IoC;

namespace Gaia.Portal.Framework
{
	public abstract class GaiaModuleAreaRegistration : AreaRegistration
	{
		private readonly string[] _nameSpaces;
		private WebModuleConfigurationAttribute _moduleConfiguration;

		protected GaiaModuleAreaRegistration(string areaName, string defaultController, string defaultAction,
			string[] namespaces = null)
		{
			if (string.IsNullOrEmpty(areaName)) throw new ArgumentNullException(areaName);
			if (string.IsNullOrEmpty(defaultController)) throw new ArgumentNullException(defaultController);
			if (string.IsNullOrEmpty(defaultAction)) throw new ArgumentNullException(defaultAction);

			_moduleConfiguration = GetType().GetCustomAttribute<WebModuleConfigurationAttribute>();

			AreaName = areaName;
			_nameSpaces = namespaces;
			DefaultController = defaultController;
			DefaultAction = defaultAction;
		}

		protected string DefaultController { get; }
		protected string DefaultAction { get; }
		public override string AreaName { get; }

		public override void RegisterArea(AreaRegistrationContext context)
		{
			RegisterContainer();

			var constraintResolver = new DefaultInlineConstraintResolver();
			constraintResolver.ConstraintMap.Add("rangeWithStatus", typeof (RangeWithStatusRouteConstraint));
			context.Routes.MapMvcAttributeRoutes(constraintResolver);

			context.Routes.MapHttpRoute(
				$"{AreaName}_DefaultApi",
				$"{AreaName}_Api/{{controller}}/{{id}}",
				new {id = RouteParameter.Optional},
				new {id = new RangeWithStatusRouteConstraint(2, 10, HttpStatusCode.PreconditionFailed)});

			context.MapRoute(
				$"{AreaName}_Area",
				$"{AreaName}/{{controller}}/{{action}}/{{id}}",
				new {area = AreaName, controller = DefaultController, action = DefaultAction, id = UrlParameter.Optional},
				_nameSpaces
				);
			RegisterRoutes(context);
		}

		public abstract void RegisterRoutes(AreaRegistrationContext context);

		private void RegisterContainer()
		{
			if (_moduleConfiguration != null)
			{
				ContainerActivator.RegisterModuleContainer(_moduleConfiguration.ModuleName);
			}
		}
	}
}