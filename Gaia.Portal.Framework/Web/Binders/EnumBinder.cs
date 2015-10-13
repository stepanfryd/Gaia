using System;
using System.Web.Mvc;

namespace Gaia.Portal.Framework.Web.Binders
{
	public class EnumBinder<T> : IModelBinder
	{
		#region Interface Implementations

		public object BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
		{
			var valueProviderResult = modelBindingContext.ValueProvider.GetValue(modelBindingContext.ModelName);
			if (string.IsNullOrEmpty(valueProviderResult.AttemptedValue)) return default(T);

			try
			{
				return (T) Enum.Parse(typeof (T), valueProviderResult.AttemptedValue);
			}
			catch
			{
				return default(T);
			}
		}

		#endregion
	}

	public class EnumBinderNullableBinder<T> : IModelBinder
	{
		#region Interface Implementations

		public object BindModel(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
		{
			var valueProviderResult = modelBindingContext.ValueProvider.GetValue(modelBindingContext.ModelName);
			if (string.IsNullOrEmpty(valueProviderResult?.AttemptedValue)) return null;

			try
			{
				return (T) Enum.Parse(typeof (T), valueProviderResult.AttemptedValue);
			}
			catch
			{
				return default(T);
			}
		}

		#endregion
	}
}