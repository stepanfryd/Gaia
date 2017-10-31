using System;
using System.Collections.Generic;
using Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Gaia.Core.IoC.Unity
{
	public class Container : IContainer
	{
		#region Private Fields

		private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		#endregion Private Fields

		#region Public Properties

		public string ConfigSource { get; set; }

		public object ContainerInstance => Instance;

		public IUnityContainer Instance => LazyContainer.Value;

		public ConfigSourceType ConfigSourceType { get ; set ; }

		#endregion Public Properties

		#region Public Methods

		public static void RegisterTypes(IUnityContainer unityContainer)
		{
			unityContainer.LoadConfiguration();
		}

		public void Dispose()
		{
			Instance.Dispose();
		}

		public bool IsRegistered<T>()
		{
			return Instance.IsRegistered<T>();
		}

		public bool IsRegistered(Type typeToCheck)
		{
			return Instance.IsRegistered(typeToCheck);
		}

		public bool IsRegistered(Type typeToCheck, string nameToCheck)
		{
			return Instance.IsRegistered(typeToCheck, nameToCheck);
		}

		public bool IsRegistered<T>(string nameToCheck)
		{
			return Instance.IsRegistered<T>(nameToCheck);
		}

		public object Resolve(Type t, string name)
		{
			return Instance.Resolve(t, name);
		}

		public T Resolve<T>()
		{
			return Instance.Resolve<T>();
		}

		public object Resolve(Type t)
		{
			return Instance.Resolve(t);
		}

		public T Resolve<T>(string name)
		{
			return Instance.Resolve<T>(name);
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return Instance.ResolveAll<T>();
		}

		#endregion Public Methods
	}
}