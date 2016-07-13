using Gaia.Core.IoC.LifetimeManagers;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Gaia.Core.IoC
{
	/// <summary>
	/// Interface specified container functions specification
	/// </summary>
	public interface IContainer : IDisposable
	{
		#region Public Properties

		object ContainerInstance { get; }

		#endregion Public Properties

		#region Public Methods

		IContainer AddNewExtension<T>();

		object BuildUp(Type t, object existing, params object[] resolverOverrides);

		IContainer CreateChildContainer();

		ILifetimeManager GetContainerControlledLifetimeManager();

		// Summary: Check if a particular type has been registered with the container with the
		// default name.
		//
		// Parameters: container: Container to inspect.
		//
		// typeToCheck: Type to check registration for.
		//
		// Returns: True if this type has been registered, false if not.
		bool IsRegistered(Type typeToCheck);

		// Summary: Check if a particular type/name pair has been registered with the container.
		//
		// Parameters: container: Container to inspect.
		//
		// typeToCheck: Type to check registration for.
		//
		// nameToCheck: Name to check registration for.
		//
		// Returns: True if this type/name pair has been registered, false if not.
		bool IsRegistered(Type typeToCheck, string nameToCheck);

		// Summary: Check if a particular type has been registered with the container with the
		// default name.
		//
		// Parameters: container: Container to inspect.
		//
		// Type parameters: T: Type to check registration for.
		//
		// Returns: True if this type has been registered, false if not.
		bool IsRegistered<T>();

		// Summary: Check if a particular type/name pair has been registered with the container.
		//
		// Parameters: container: Container to inspect.
		//
		// nameToCheck: Name to check registration for.
		//
		// Type parameters: T: Type to check registration for.
		//
		// Returns: True if this type/name pair has been registered, false if not.
		bool IsRegistered<T>(string nameToCheck);

		object RegisterChildContainer(Configuration configuration, string childName);

		object RegisterInstance<TInterface>(TInterface instance);

		object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager);

		object RegisterInstance<TInterface>(string name, TInterface instance);

		object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager);

		object RegisterInstance(Type t, object instance);

		object RegisterInstance(Type t, object instance, object lifetimeManager);

		object RegisterInstance(Type t, string name, object instance);

		IContainer RegisterType<TFrom, TTo>(ILifetimeManager lifetimeManager);

		IContainer RegisterType<T>(params IInjectionMember[] lifetimeManager);

		object Resolve(Type t, string name);

		T Resolve<T>();

		T Resolve<T>(string name);

		object Resolve(Type t);

		IEnumerable<object> ResolveAll(Type t);

		IEnumerable<T> ResolveAll<T>();

		#endregion Public Methods
	}
}