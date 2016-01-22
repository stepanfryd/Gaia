using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Gaia.Core.IoC.Unity
{
	public class Container : IContainer
	{
		private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});


		public IUnityContainer Instance => LazyContainer.Value;

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

		public object BuildUp(Type t, object existing, params object[] resolverOverrides)
		{
			return Instance.BuildUp(t, existing);
		}

		public object RegisterChildContainer(Configuration configuration, string childName)
		{
			var unitySection = (UnityConfigurationSection) configuration.GetSection("unity");

			var subContainer = Instance.CreateChildContainer();
			subContainer.LoadConfiguration(unitySection, childName);
			return Instance.RegisterInstance(typeof (IUnityContainer), childName, subContainer);
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

		public void Dispose()
		{
			Instance.Dispose();
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration and has the container take over the lifetime of the instance.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
		/// <param name="instance">Object to returned.</param>
		public object RegisterInstance<TInterface>(TInterface instance)
		{
			return Instance.RegisterInstance(typeof (TInterface), null, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload automatically has the container take ownership of the <paramref name="instance" />.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
		/// <param name="instance">Object to returned.</param>
		/// <param name="name">Name for registration.</param>
		/// <returns>
		///   The <see cref="T:Microsoft.Practices.Unity.UnityContainer" /> object that this method was called on (this in C#, Me
		///   in Visual Basic).
		/// </returns>
		public object RegisterInstance<TInterface>(string name, TInterface instance)
		{
			return Instance.RegisterInstance(typeof (TInterface), name, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration and has the container take over the lifetime of the instance.
		///   </para>
		/// </remarks>
		/// <param name="t">Type of instance to register (may be an implemented interface instead of the full type).</param>
		/// <param name="instance">Object to returned.</param>
		public object RegisterInstance(Type t, object instance)
		{
			return Instance.RegisterInstance(t, null, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload automatically has the container take ownership of the <paramref name="instance" />.
		///   </para>
		/// </remarks>
		public object RegisterInstance(Type t, string name, object instance)
		{
			return Instance.RegisterInstance(t, name, instance, new ContainerControlledLifetimeManager());
		}

		/// <summary>
		///   Register a type mapping with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method is used to tell the container that when asked for type <typeparamref name="TFrom" />,
		///     actually return an instance of type <typeparamref name="TTo" />. This is very useful for
		///     getting instances of interfaces.
		///   </para>
		///   <para>
		///     This overload registers a default mapping and transient lifetime.
		///   </para>
		/// </remarks>
		/// <typeparam name="TFrom"><see cref="T:System.Type" /> that will be requested.</typeparam>
		/// <typeparam name="TTo"><see cref="T:System.Type" /> that will actually be returned.</typeparam>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<TFrom, TTo>(params object[] injectionMembers) where TTo : TFrom
		{
			return RegisterType(typeof (TFrom), typeof (TTo), null, null, injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the container, where the created instances will use
		///   the given <see cref="T:Microsoft.Practices.Unity.LifetimeManager" />.
		/// </summary>
		/// <typeparam name="TFrom"><see cref="T:System.Type" /> that will be requested.</typeparam>
		/// <typeparam name="TTo"><see cref="T:System.Type" /> that will actually be returned.</typeparam>
		/// <param name="lifetimeManager">
		///   The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
		///   of the returned instance.
		/// </param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<TFrom, TTo>(object lifetimeManager,
			params object[] injectionMembers) where TTo : TFrom
		{
			return Instance.RegisterType(typeof (TFrom), typeof (TTo), null, (LifetimeManager) lifetimeManager,
				(InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the Instance.
		/// </summary>
		/// <remarks>
		///   This method is used to tell the container that when asked for type <typeparamref name="TFrom" />,
		///   actually return an instance of type <typeparamref name="TTo" />. This is very useful for
		///   getting instances of interfaces.
		/// </remarks>
		/// <typeparam name="TFrom"><see cref="T:System.Type" /> that will be requested.</typeparam>
		/// <typeparam name="TTo"><see cref="T:System.Type" /> that will actually be returned.</typeparam>
		/// <param name="name">Name of this mapping.</param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<TFrom, TTo>(string name, params object[] injectionMembers)
			where TTo : TFrom
		{
			return Instance.RegisterType(typeof (TFrom), typeof (TTo), name, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the container, where the created instances will use
		///   the given <see cref="T:Microsoft.Practices.Unity.LifetimeManager" />.
		/// </summary>
		/// <typeparam name="TFrom"><see cref="T:System.Type" /> that will be requested.</typeparam>
		/// <typeparam name="TTo"><see cref="T:System.Type" /> that will actually be returned.</typeparam>
		/// <param name="name">Name to use for registration, null if a default registration.</param>
		/// <param name="lifetimeManager">
		///   The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
		///   of the returned instance.
		/// </param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<TFrom, TTo>(string name, object lifetimeManager,
			params object[] injectionMembers) where TTo : TFrom
		{
			return Instance.RegisterType(typeof (TFrom), typeof (TTo), name, (LifetimeManager) lifetimeManager,
				(InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		/// <typeparam name="T">The type to apply the <paramref name="lifetimeManager" /> to.</typeparam>
		/// <param name="lifetimeManager">
		///   The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
		///   of the returned instance.
		/// </param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<T>(object lifetimeManager, params object[] injectionMembers)
		{
			return Instance.RegisterType(null, typeof (T), null, (LifetimeManager) lifetimeManager,
				(InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		/// <typeparam name="T">The type to configure injection on.</typeparam>
		/// <param name="name">Name that will be used to request the type.</param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<T>(string name, params object[] injectionMembers)
		{
			return Instance.RegisterType(null, typeof (T), name, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type and name with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		/// <typeparam name="T">The type to apply the <paramref name="lifetimeManager" /> to.</typeparam>
		/// <param name="name">Name that will be used to request the type.</param>
		/// <param name="lifetimeManager">
		///   The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
		///   of the returned instance.
		/// </param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<T>(string name, object lifetimeManager,
			params object[] injectionMembers)
		{
			return Instance.RegisterType(null, typeof (T), name, (LifetimeManager) lifetimeManager,
				(InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type with specific members to be injected.
		/// </summary>
		/// <param name="t">Type this registration is for.</param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType(Type t, params object[] injectionMembers)
		{
			return Instance.RegisterType(null, t, null, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type with specific members to be injected.
		/// </summary>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<T>(params object[] injectionMembers)
		{
			return Instance.RegisterType(null, typeof (T), null, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method is used to tell the container that when asked for type <paramref name="from" />,
		///     actually return an instance of type <paramref name="to" />. This is very useful for
		///     getting instances of interfaces.
		///   </para>
		///   <para>
		///     This overload registers a default mapping.
		///   </para>
		/// </remarks>
		/// <param name="from"><see cref="T:System.Type" /> that will be requested.</param>
		/// <param name="to"><see cref="T:System.Type" /> that will actually be returned.</param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType(Type from, Type to, params object[] injectionMembers)
		{
			return Instance.RegisterType(from, to, null, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the Instance.
		/// </summary>
		/// <remarks>
		///   This method is used to tell the container that when asked for type <paramref name="from" />,
		///   actually return an instance of type <paramref name="to" />. This is very useful for
		///   getting instances of interfaces.
		/// </remarks>
		/// <param name="from"><see cref="T:System.Type" /> that will be requested.</param>
		/// <param name="to"><see cref="T:System.Type" /> that will actually be returned.</param>
		/// <param name="name">Name to use for registration, null if a default registration.</param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType(Type from, Type to, string name, params object[] injectionMembers)
		{
			return Instance.RegisterType(from, to, name, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a type mapping with the container, where the created instances will use
		///   the given <see cref="T:Microsoft.Practices.Unity.LifetimeManager" />.
		/// </summary>
		/// <param name="from"><see cref="T:System.Type" /> that will be requested.</param>
		/// <param name="to"><see cref="T:System.Type" /> that will actually be returned.</param>
		/// <param name="lifetimeManager">
		///   The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
		///   of the returned instance.
		/// </param>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType(Type from, Type to, object lifetimeManager,
			params object[] injectionMembers)
		{
			return Instance.RegisterType(from, to, null, (LifetimeManager) lifetimeManager, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type and name with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		public object RegisterType(Type t, object lifetimeManager, params object[] injectionMembers)
		{
			return Instance.RegisterType(null, t, null, (LifetimeManager) lifetimeManager, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type and name with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		public object RegisterType(Type t, string name, params object[] injectionMembers)
		{
			return Instance.RegisterType(null, t, name, null, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register a <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> for the given type and name with the Instance.
		///   No type mapping is performed for this type.
		/// </summary>
		public object RegisterType(Type t, string name, object lifetimeManager,
			params object[] injectionMembers)
		{
			return Instance.RegisterType(null, t, name, (LifetimeManager) lifetimeManager, (InjectionMember[]) injectionMembers);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration (name = null).
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
		/// <param name="instance">Object to returned.</param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(typeof (TInterface), null, instance, (LifetimeManager) lifetimeManager);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
		/// <param name="instance">Object to returned.</param>
		/// <param name="name">Name for registration.</param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(typeof (TInterface), name, instance, (LifetimeManager) lifetimeManager);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead
		///     of the container creating the instance the first time it is requested, the user
		///     creates the instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration (name = null).
		///   </para>
		/// </remarks>
		/// <param name="t">Type of instance to register (may be an implemented interface instead of the full type).</param>
		/// <param name="instance">Object to returned.</param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance(Type t, object instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(t, null, instance, (LifetimeManager) lifetimeManager);
		}

		/// <summary>
		///   Register a type with specific members to be injected.
		/// </summary>
		/// <typeparam name="T">Type this registration is for.</typeparam>
		/// <param name="injectionMembers">Injection configuration objects.</param>
		public object RegisterType<T>(params InjectionMember[] injectionMembers)
		{
			return Instance.RegisterType(null, typeof (T), null, null, injectionMembers);
		}

		public static void RegisterTypes(IUnityContainer unityContainer)
		{
			unityContainer.LoadConfiguration();
		}

		private LifetimeManager CreateDefaultInstanceLifetimeManager()
		{
			return new ContainerControlledLifetimeManager();
		}
	}
}