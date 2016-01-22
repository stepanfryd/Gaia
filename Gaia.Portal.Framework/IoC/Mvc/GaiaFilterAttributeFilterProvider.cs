using System.Collections.Generic;
using System.Web.Mvc;
using Gaia.Core.IoC;
// ReSharper disable PossibleMultipleEnumeration

namespace Gaia.Portal.Framework.IoC.Mvc
{
	/// <summary>
	///   Defines a filter provider for filter attributes that support injection of Unity dependencies.
	/// </summary>
	public class GaiaFilterAttributeFilterProvider : FilterAttributeFilterProvider
	{
		private readonly IContainer _container;

		/// <summary>
		///   Initializes a new instance of the <see cref="T:Gaia.Portal.Framework.IoC.GaiaFilterAttributeFilterProvider" />
		///   class.
		/// </summary>
		/// <param name="container">
		///   The <see cref="T:Gaia.Core.IoC.IContainer" /> that will be used to inject the
		///   filters.
		/// </param>
		public GaiaFilterAttributeFilterProvider(IContainer container)
		{
			_container = container;
		}

		/// <summary>
		///   Gets a collection of custom action attributes, and injects them using a Unity container.
		/// </summary>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>
		///   A collection of custom action attributes.
		/// </returns>
		protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext,
			ActionDescriptor actionDescriptor)
		{
			var actionAttributes = base.GetActionAttributes(controllerContext, actionDescriptor);
			foreach (var filterAttribute in actionAttributes)
			{
				_container.BuildUp(filterAttribute.GetType(), filterAttribute);
			}
			return actionAttributes;
		}

		/// <summary>
		///   Gets a collection of controller attributes, and injects them using a Unity container.
		/// </summary>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>
		///   A collection of controller attributes.
		/// </returns>
		protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext,
			ActionDescriptor actionDescriptor)
		{
			var controllerAttributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
			foreach (var filterAttribute in controllerAttributes)
			{
				_container.BuildUp(filterAttribute.GetType(), filterAttribute);
			}
			return controllerAttributes;
		}
	}
}