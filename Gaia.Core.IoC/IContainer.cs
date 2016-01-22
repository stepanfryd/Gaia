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

		object RegisterType<T>(params object[] injectionMembers);

		object RegisterType<TFrom, TTo>(params object[] injectionMembers) where TTo : TFrom;

		object RegisterType<TFrom, TTo>(object lifetimeManager, params object[] injectionMembers)
			where TTo : TFrom;

		object RegisterType<TFrom, TTo>(string name, params object[] injectionMembers)
			where TTo : TFrom;

		object RegisterType<TFrom, TTo>(string name, object lifetimeManager,
			params object[] injectionMembers) where TTo : TFrom;

		object RegisterType<T>(object lifetimeManager, params object[] injectionMembers);
		object RegisterType<T>(object lifetimeManager);

		object RegisterType<T>(string name, params object[] injectionMembers);

		object RegisterType<T>(string name, object lifetimeManager,
			params object[] injectionMembers);

		object RegisterType(Type t, params object[] injectionMembers);

		object RegisterType(Type from, Type to, params object[] injectionMembers);

		object RegisterType(Type from, Type to, string name, params object[] injectionMembers);

		object RegisterType(Type from, Type to, object lifetimeManager,
			params object[] injectionMembers);

		object RegisterType(Type t, object lifetimeManager, params object[] injectionMembers);

		object RegisterType(Type t, string name, params object[] injectionMembers);

		object RegisterType(Type t, string name, object lifetimeManager,
			params object[] injectionMembers);

		object RegisterInstance<TInterface>(TInterface instance);

		object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager);

		object RegisterInstance<TInterface>(string name, TInterface instance);

		object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager);

		object RegisterInstance(Type t, object instance);

		object RegisterInstance(Type t, object instance, object lifetimeManager);

		object RegisterInstance(Type t, string name, object instance);
	}
}