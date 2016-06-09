/*
 Code copy from https://github.com/dimaKudr/Unity.WF
 */

using System;
using Microsoft.Practices.Unity;

namespace Gaia.Core.Workflows.IoC
{
	public sealed class DependencyInjectionExtension
	{
		#region Fields and constants

		private readonly IUnityContainer _container;

		#endregion

		#region Constructors

		public DependencyInjectionExtension(IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException(nameof(container));

			_container = container;
		}

		#endregion

		#region Private and protected

		public T GetDependency<T>()
		{
			return _container.Resolve<T>();
		}

		#endregion
	}
}