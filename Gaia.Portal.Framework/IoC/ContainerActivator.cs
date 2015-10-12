using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Gaia.Core.IoC;
using Gaia.Portal.Framework.Configuration.Modules;
using Gaia.Portal.Framework.Exceptions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Mvc;

namespace Gaia.Portal.Framework.IoC
{
	public static class ContainerActivator
	{
		/// <summary>Integrates Unity when the application starts.</summary>
		public static void Start()
		{
			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(Container.Instance));

			DependencyResolver.SetResolver(new UnityDependencyResolver(Container.Instance));

			// TODO: Uncomment if you want to use PerRequestLifetimeManager
			// Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

			// WebApi dependency configuration
			var depRes = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(Container.Instance);
			GlobalConfiguration.Configuration.DependencyResolver = depRes;
		}

		/// <summary>Disposes the Unity container when the application is shut down.</summary>
		public static void Shutdown()
		{
			Container.Instance.Dispose();
		}

		/// <summary>
		///   Load Unity IoC container configuration from module configuratoin file
		/// </summary>
		public static void RegisterModuleContainer(string moduleName)
		{
			var module = Container.Instance.Resolve<IWebModuleProvider>()?.Modules?.SingleOrDefault(m => m.Name == moduleName);
			if (string.IsNullOrEmpty(module?.IocContainerFullPath) || !File.Exists(module.IocContainerFullPath)) return;

			try
			{
				var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = module.IocContainerFullPath};
				var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
				var unitySection = (UnityConfigurationSection) configuration.GetSection("unity");
				var subContainer = Container.Instance.CreateChildContainer();
				subContainer.LoadConfiguration(unitySection, module.Name);
				Container.Instance.RegisterInstance(typeof(IUnityContainer), module.Name, subContainer);
			}
			catch (Exception e)
			{
				throw new DependencyContainerLoadingException(
					$"Unable to load module IoC container configuration from file {module.IocContainerFullPath}.", e);
			}
		}
	}
}