using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Resolution;

namespace Gaia.Core.IoC.Unity
{
	public class Container : IContainer<IUnityContainer>
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
		public ConfigSourceType ConfigSourceType { get; set; }

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

		public object Resolve(Type t, string name, IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve(t, name, GetParameters(parameters));
		}

		public T Resolve<T>(IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve<T>(GetParameters(parameters));
		}

		public object Resolve(Type t, IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve(t, GetParameters(parameters));
		}

		public T Resolve<T>(string name, IDictionary<string, object> parameters = null)
		{
			return Instance.Resolve<T>(name, GetParameters(parameters));
		}

		public IEnumerable<T> ResolveAll<T>(IDictionary<string, object> parameters = null)
		{
			return Instance.ResolveAll<T>(GetParameters(parameters));
		}

		#endregion Public Methods

		private ResolverOverride[] GetParameters(IDictionary<string, object> parameters)
		{
			if (parameters == null || parameters.Count == 0) return null;

			return parameters.Select(i => new ParameterOverride(i.Key, i.Value)).ToArray();
		}
	}
}