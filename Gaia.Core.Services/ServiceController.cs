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
	public class ServiceController<T> : IDisposable, ServiceControl, ServiceSuspend, ServiceShutdown where T : IGaiaService
	{
		#region Constructors

		public ServiceController(T service, PluginConfigurationCollection pluginsConfiguration,
			ServiceHostConfigurationCollection wcfServicesConfiguration)
		{
			_service = service;
			_pluginsManager = new PluginsManager(pluginsConfiguration);
			_servicesManager = new WcfServicesManager(wcfServicesConfiguration);
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

		#region Private members

		private readonly IGaiaService _service;
		private readonly PluginsManager _pluginsManager;
		private readonly WcfServicesManager _servicesManager;

		#endregion

		#region Public properties

		#endregion

		#region Public methods

		public bool Start(HostControl hostControl)
		{
			InitServices();
			InitPlugins();
			_service.Start();
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			KillPlugins();
			ShutDownServices();
			KillDomains();
			_service.Stop();
			return true;
		}

		public bool Continue(HostControl hostControl)
		{
			_service.Continue();
			return true;
		}

		public bool Pause(HostControl hostControl)
		{
			_service.Pause();
			return true;
		}

		public void Shutdown(HostControl hostControl)
		{
			_service.Shutdown();
		}

		#endregion

		#region Private methods

		private void InitServices()
		{
			_servicesManager.InitalizeServices();
		}

		private void ShutDownServices()
		{
			if (_servicesManager != null)
			{
				_servicesManager.ShutdownServices();
				_servicesManager.Dispose();
				GC.SuppressFinalize(_servicesManager);
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
			_pluginsManager.InitalizePlugins();
		}

		private void KillPlugins()
		{
			_pluginsManager.Dispose();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ShutDownServices();
				KillDomains();
			}
		}

		#endregion
	}
}