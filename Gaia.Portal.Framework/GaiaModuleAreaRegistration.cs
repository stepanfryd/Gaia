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