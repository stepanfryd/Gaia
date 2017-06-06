/*
The MIT License (MIT)

Copyright (c) 2012 Stepan Fryd (stepan.fryd@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Gaia.Core.IoC;
using Gaia.Portal.Framework.Configuration.Modules;
using Gaia.Portal.Framework.Exceptions;

namespace Gaia.Portal.Framework.IoC
{
	/// <summary>
	///   IoC container activator
	/// </summary>
	public static class ContainerActivator
	{
		/// <summary>Integrates Unity when the application starts.</summary>
		public static void Start()
		{
			// init entLib logger
			var config = new Configuration.Configuration();
			if(config.WebSettings==null) throw new NullReferenceException("Configuration section gaia/web is not correctly configured.");

			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			
			if (config.WebSettings.FilterProviders != null)
			{
				foreach (var type in config.WebSettings.FilterProviders.Select(Type.GetType).Where(type => type != null))
				{
					FilterProviders.Providers.Add((IFilterProvider) Activator.CreateInstance(type, Container.Instance.ContainerInstance));
				}
			}

			var mvcResolver = Type.GetType(config.WebSettings.MvcDependencyResolver);
			if (mvcResolver == null)
			{
				throw new NullReferenceException($"Could not resolve type ${config.WebSettings.MvcDependencyResolver} for Mvc Dependency resolver.");
			}

			DependencyResolver.SetResolver(
				(IDependencyResolver) Activator.CreateInstance(mvcResolver, Container.Instance.ContainerInstance));

			// TODO: Uncomment if you want to use PerRequestLifetimeManager
			// Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

			var webApiResolver = Type.GetType(config.WebSettings.ApiDependencyResolver);
			if (webApiResolver == null)
				throw new NullReferenceException(
					$"Cound not resolve ${config.WebSettings.ApiDependencyResolver} for WebApi Dependency resolver.");
			// WebApi dependency configuration
			GlobalConfiguration.Configuration.DependencyResolver =
				(System.Web.Http.Dependencies.IDependencyResolver)
					Activator.CreateInstance(webApiResolver, Container.Instance.ContainerInstance);
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
				Container.Instance.RegisterChildContainer(configuration, module.Name);
			}
			catch (Exception e)
			{
				throw new DependencyContainerLoadingException(
					$"Unable to load module IoC container configuration from file {module.IocContainerFullPath}.", e);
			}
		}
	}
}