using System;
using System.Collections.Generic;
using System.Configuration;
using Gaia.Core.IoC.LifetimeManagers;

namespace Gaia.Core.IoC
{
	/// <summary>
	///   Interface specified container functions specification
	/// </summary>
	public interface IContainer : IDisposable
	{
		#region Public members

		object ContainerInstance { get; }

		#endregion

		#region Private and protected

		object Resolve(Type t, string name);

		IEnumerable<object> ResolveAll(Type t);

		T Resolve<T>();

		T Resolve<T>(string name);

		object Resolve(Type t);

		IEnumerable<T> ResolveAll<T>();

		object BuildUp(Type t, object existing, params object[] resolverOverrides);

		object RegisterChildContainer(Configuration configuration, string childName);

		IContainer CreateChildContainer();

		object RegisterInstance<TInterface>(TInterface instance);

		object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager);

		object RegisterInstance<TInterface>(string name, TInterface instance);

		object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager);

		object RegisterInstance(Type t, object instance);

		object RegisterInstance(Type t, object instance, object lifetimeManager);

		object RegisterInstance(Type t, string name, object instance);

		IContainer RegisterType<TFrom, TTo>(ILifetimeManager lifetimeManager);

		IContainer RegisterType<T>(params IInjectionMember[] lifetimeManager);

		ILifetimeManager GetContainerControlledLifetimeManager();

		#endregion
	}
}