using System;
using System.Collections.Generic;
using System.Configuration;

namespace Gaia.Core.IoC
{
	/// <summary>
	///   Interface specified container functions specification
	/// </summary>
	public interface IContainer : IDisposable
	{
		object ContainerInstance { get; }

		object Resolve(Type t, string name);

		IEnumerable<object> ResolveAll(Type t);

		T Resolve<T>();

		T Resolve<T>(string name);

		object Resolve(Type t);

		IEnumerable<T> ResolveAll<T>();

		object BuildUp(Type t, object existing, params object[] resolverOverrides);

		object RegisterChildContainer(Configuration configuration, string childName);


		object RegisterInstance<TInterface>(TInterface instance);

		object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager);

		object RegisterInstance<TInterface>(string name, TInterface instance);

		object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager);

		object RegisterInstance(Type t, object instance);

		object RegisterInstance(Type t, object instance, object lifetimeManager);

		object RegisterInstance(Type t, string name, object instance);
	}
}