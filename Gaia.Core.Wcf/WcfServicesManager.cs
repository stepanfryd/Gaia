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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Common.Logging;
using Gaia.Core.Wcf.Configuration;

namespace Gaia.Core.Wcf
{
	/// <summary>
	///   WCF services manager class for initialization and shutdown from configuration
	/// </summary>
	[Serializable]
	public class WcfServicesManager : IDisposable
	{
		#region Constructors

		/// <summary>
		///   Manager constructor
		/// </summary>
		/// <param name="configurationCollection">List of services to be managed by this manager</param>
		public WcfServicesManager(ServiceHostConfigurationCollection configurationCollection)
		{
			_log = LogManager.GetLogger(GetType());
			_serviceHosts = new List<ServiceHostBase>();
			_servicesConfiguration = configurationCollection;
		}

		#endregion

		#region Private and protected

		/// <summary>
		///   Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
		///   collection.
		/// </summary>
		~WcfServicesManager()
		{
			Dispose(false);
		}

		#endregion

		#region Private members

		private readonly IList<ServiceHostBase> _serviceHosts;
		private readonly ServiceHostConfigurationCollection _servicesConfiguration;
		private ILog _log;

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

		/// <summary>
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ShutdownServices();
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		///   Initialize all configured services
		/// </summary>
		public void InitalizeServices()
		{
			if (_servicesConfiguration == null || _servicesConfiguration.Count == 0)
			{
				_log.Info("No services are configured.");
				return;
			}

			foreach (IServiceHostConfiguration hostConfig in _servicesConfiguration)
			{
				var factory = Activator.CreateInstance(hostConfig.FactoryType);

				var host = ((ServiceHostFactoryBase)factory).CreateServiceHost(hostConfig);
				host.Open();

				var firstOrDefault = host.BaseAddresses.FirstOrDefault();
				if (firstOrDefault != null)
				{
					_log.Info($"Service {hostConfig.ServiceTypeName} on {firstOrDefault.AbsoluteUri}");
				}

				_serviceHosts.Add(host);
			}
		}

		/// <summary>
		///   Shutdown all services which are up and running
		/// </summary>
		public void ShutdownServices()
		{
			if (_serviceHosts != null)
			{
				foreach (var host in _serviceHosts)
				{
					host.Close();
				}

				_serviceHosts.Clear();
			}
		}

		#endregion
	}
}