using System;
using System.Collections.Generic;
using System.Configuration;
using Gaia.Core.IoC.LifetimeManagers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using ContainerControlledLifetimeManager = Gaia.Core.IoC.Unity.LifetimeManagers.ContainerControlledLifetimeManager;
using HierarchicalLifetimeManager = Gaia.Core.IoC.Unity.LifetimeManagers.HierarchicalLifetimeManager;
using TransientLifetimeManager = Gaia.Core.IoC.Unity.LifetimeManagers.TransientLifetimeManager;

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

		public object ContainerInstance => Instance;

		public IUnityContainer Instance => LazyContainer.Value;

		#endregion Public Properties

		#region Public Methods

		public static void RegisterTypes(IUnityContainer unityContainer)
		{
			unityContainer.LoadConfiguration();
		}

		public IContainer AddNewExtension<T>()
		{
			var extension = Instance.Resolve<T>() as UnityContainerExtension;
			Instance.AddExtension(extension);
			return this;
		}

		public object BuildUp(Type t, object existing, params object[] resolverOverrides)
		{
			return Instance.BuildUp(t, existing);
		}

		public IContainer CreateChildContainer()
		{
			return new Container();
		}

		public void Dispose()
		{
			Instance.Dispose();
		}

		public ILifetimeManager GetContainerControlledLifetimeManager()
		{
			return new ContainerControlledLifetimeManager();
		}

		public ILifetimeManager GetTransientLifetimeManager()
		{
			return new TransientLifetimeManager();
		}

		public ILifetimeManager GetHierarchicalLifetimeManager()
		{
			return new HierarchicalLifetimeManager();
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

		public object RegisterChildContainer(Configuration configuration, string childName)
		{
			var unitySection = (UnityConfigurationSection) configuration.GetSection("unity");

			var subContainer = Instance.CreateChildContainer();
			subContainer.LoadConfiguration(unitySection, childName);
			return Instance.RegisterInstance(typeof(IUnityContainer), childName, subContainer);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration and has the container take over the lifetime of
		///     the instance.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </typeparam>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		public object RegisterInstance<TInterface>(TInterface instance)
		{
			return Instance.RegisterInstance(typeof(TInterface), null, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload automatically has the container take ownership of the <paramref name="instance" />.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </typeparam>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		/// <param name="name">
		///   Name for registration.
		/// </param>
		/// <returns>
		///   The <see cref="T:Microsoft.Practices.Unity.UnityContainer" /> object that this method was
		///   called on (this in C#, Me in Visual Basic).
		/// </returns>
		public object RegisterInstance<TInterface>(string name, TInterface instance)
		{
			return Instance.RegisterInstance(typeof(TInterface), name, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration and has the container take over the lifetime of
		///     the instance.
		///   </para>
		/// </remarks>
		/// <param name="t">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </param>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		public object RegisterInstance(Type t, object instance)
		{
			return Instance.RegisterInstance(t, null, instance, CreateDefaultInstanceLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload automatically has the container take ownership of the <paramref name="instance" />.
		///   </para>
		/// </remarks>
		public object RegisterInstance(Type t, string name, object instance)
		{
			return Instance.RegisterInstance(t, name, instance,
				new Microsoft.Practices.Unity.ContainerControlledLifetimeManager());
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration (name = null).
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </typeparam>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance<TInterface>(TInterface instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(typeof(TInterface), null, instance, (LifetimeManager) lifetimeManager);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		/// </remarks>
		/// <typeparam name="TInterface">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </typeparam>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		/// <param name="name">
		///   Name for registration.
		/// </param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance<TInterface>(string name, TInterface instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(typeof(TInterface), name, instance, (LifetimeManager) lifetimeManager);
		}

		/// <summary>
		///   Register an instance with the Instance.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Instance registration is much like setting a type as a singleton, except that instead of
		///     the container creating the instance the first time it is requested, the user creates the
		///     instance ahead of type and adds that instance to the Instance.
		///   </para>
		///   <para>
		///     This overload does a default registration (name = null).
		///   </para>
		/// </remarks>
		/// <param name="t">
		///   Type of instance to register (may be an implemented interface instead of the full type).
		/// </param>
		/// <param name="instance">
		///   Object to returned.
		/// </param>
		/// <param name="lifetimeManager">
		///   <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> object that controls how this
		///   instance will be managed by the Instance.
		/// </param>
		public object RegisterInstance(Type t, object instance, object lifetimeManager)
		{
			return Instance.RegisterInstance(t, null, instance, (LifetimeManager) lifetimeManager);
		}

		public IContainer RegisterType<TFrom, TTo>(ILifetimeManager lifetimeManager)
		{
			Instance.RegisterType(typeof(TFrom), typeof(TTo), (LifetimeManager) lifetimeManager);
			return this;
		}

		public IContainer RegisterType<T>(params IInjectionMember[] injectionMembers)
		{
			Instance.RegisterType(typeof(T), GetInjectionMembers(injectionMembers));
			return this;
		}

		public IContainer RegisterType(Type t, params IInjectionMember[] injectionMembers)
		{
			Instance.RegisterType(t, GetInjectionMembers(injectionMembers));
			return this;
		}

		public IContainer RegisterType(Type t)
		{
			Instance.RegisterType(t);
			return this;
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

		public IEnumerable<object> ResolveAll(Type t)
		{
			return Instance.ResolveAll(t);
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return Instance.ResolveAll<T>();
		}

		#endregion Public Methods

		#region Private Methods

		private Microsoft.Practices.Unity.InjectionMember[] GetInjectionMembers(IInjectionMember[] injectionMembers)
		{
			var members = new List<Microsoft.Practices.Unity.InjectionMember>();

			if (injectionMembers != null)
			{
				foreach (var member in injectionMembers)
				{
					if (member.Type != null && member.Name != null)
					{
						members.Add(new InjectionFactory(container => container.Resolve(member.Type, member.Name)));
					}
					else if (member.Type != null)
					{
						members.Add(new InjectionFactory(container => container.Resolve(member.Type)));
					}
				}
			}

			return members.ToArray();
		}

		private LifetimeManager CreateDefaultInstanceLifetimeManager()
		{
			return new Microsoft.Practices.Unity.ContainerControlledLifetimeManager();
		}

		#endregion Private Methods
	}
}