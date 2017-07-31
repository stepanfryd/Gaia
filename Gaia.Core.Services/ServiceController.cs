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
using Gaia.Core.Services.Configuration;
using Gaia.Core.Wcf;
using Gaia.Core.Wcf.Configuration;
using Topshelf;

namespace Gaia.Core.Services
{
	/// <summary>
	/// Base abstract class for service controllers
	/// </summary>
	public abstract class ServiceControllerBase : IServiceController
	{
		/// <summary>
		///   ServiceController constructor creates new instatnce of service for registration
		/// </summary>
		/// <param name="service">Instance of service</param>
		/// <param name="pluginsConfiguration">Instance of plugin configuration collections</param>
		/// <param name="wcfServicesConfiguration">WCF services configuration</param>
		protected ServiceControllerBase(IGaiaService service, PluginConfigurationCollection pluginsConfiguration,
			ServiceHostConfigurationCollection wcfServicesConfiguration)
		{
			Service = service;
			PluginsManager = new PluginsManager(pluginsConfiguration);
			ServicesManager = new WcfServicesManager(wcfServicesConfiguration);

			service.ServiceController = this;
		}

		/// <summary>
		///   Controlled service
		/// </summary>
		public IGaiaService Service { get; protected set; }

		/// <summary>
		///   Instance of service plugin manager if exists
		/// </summary>
		public PluginsManager PluginsManager { get; protected set; }

		/// <summary>
		///   Instance of WCF service manager
		/// </summary>
		public WcfServicesManager ServicesManager { get; protected set; }
	}

	/// <summary>
	///   Services controller class. Controls service execution, and management
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ServiceController<T> : ServiceControllerBase, IDisposable, ServiceControl, ServiceSuspend, ServiceShutdown
		where T : IGaiaService
	{
		#region Constructors

		/// <summary>
		///   ServiceController constructor creates new instatnce of service for registration
		/// </summary>
		/// <param name="service">Instance of service</param>
		/// <param name="pluginsConfiguration">Instance of plugin configuration collections</param>
		/// <param name="wcfServicesConfiguration">WCF services configuration</param>
		public ServiceController(T service, PluginConfigurationCollection pluginsConfiguration,
			ServiceHostConfigurationCollection wcfServicesConfiguration) : base(service, pluginsConfiguration,
			wcfServicesConfiguration)
		{
		}

		#endregion

		#region Private and protected

		/// <summary>
		///   Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
		///   collection.
		/// </summary>
		~ServiceController()
		{
			Dispose(true);
		}

		#endregion


		#region Public methods

		/// <summary>
		/// </summary>
		/// <param name="hostControl"></param>
		/// <returns></returns>
		public bool Start(HostControl hostControl)
		{
			InitServices();
			InitPlugins();
			Service.Start();
			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="hostControl"></param>
		/// <returns></returns>
		public bool Stop(HostControl hostControl)
		{
			KillPlugins();
			ShutDownServices();
			KillDomains();
			Service.Stop();
			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="hostControl"></param>
		/// <returns></returns>
		public bool Continue(HostControl hostControl)
		{
			Service.Continue();
			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="hostControl"></param>
		/// <returns></returns>
		public bool Pause(HostControl hostControl)
		{
			Service.Pause();
			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="hostControl"></param>
		public void Shutdown(HostControl hostControl)
		{
			Service.Shutdown();
		}

		#endregion

		#region Private methods

		private void InitServices()
		{
			ServicesManager?.InitalizeServices();
		}

		private void ShutDownServices()
		{
			if (ServicesManager != null)
			{
				ServicesManager.ShutdownServices();
				ServicesManager.Dispose();
				GC.SuppressFinalize(ServicesManager);
			}
		}

		private void KillDomains()
		{
			//if (_hostDomain != null)
			//{
			//	//AppDomain.Unload(_hostDomain);
			//}
		}

		private void InitPlugins()
		{
			PluginsManager?.InitalizePlugins();
		}

		private void KillPlugins()
		{
			if (PluginsManager != null)
			{
				PluginsManager.ShutdownPlugins();
				PluginsManager.Dispose();
				GC.SuppressFinalize(PluginsManager);
			}
		}

		#endregion

		#region IDisposable implementation

		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				ShutDownServices();
				KillPlugins();
				KillDomains();
			}
		}

		#endregion
	}
}