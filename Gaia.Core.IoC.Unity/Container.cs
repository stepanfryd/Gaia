using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Gaia.Core.IoC.Unity
{
	public class Container : IContainerProvider
	{
		private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		public static IUnityContainer Instance => LazyContainer.Value;

		public object Resolve(Type t, string name)
		{
			return Instance.Resolve(t, name);
		}

		public IEnumerable<object> ResolveAll(Type t)
		{
			return Instance.ResolveAll(t);
		}

		public T Resolve<T>()
		{
			return Instance.Resolve<T>();
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return Instance.ResolveAll<T>();
		}

		public object ContainerInstance => Instance;

		public object Resolve(Type t)
		{
			return Instance.Resolve(t);
		}

		public T Resolve<T>(string name)
		{
			return Instance.Resolve<T>(name);
		}

		public static void RegisterTypes(IUnityContainer unityContainer)
		{
			unityContainer.LoadConfiguration();
		}
	}
}