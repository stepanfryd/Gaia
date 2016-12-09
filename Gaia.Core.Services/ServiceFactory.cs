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
using System.Reflection;
using Gaia.Core.Services.Configuration;
using Gaia.Core.Wcf.Configuration;
using Topshelf;

namespace Gaia.Core.Services
{
	/// <summary>
	///   Factory creates service based on service controll
	/// </summary>
	public class ServiceFactory
	{
		#region Public members

		/// <summary>
		///   Service description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///   Service display name
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		///   Service system name
		/// </summary>
		public string ServiceName { get; set; }

		#endregion

		#region Constructors

		private ServiceFactory()
		{
		}

		#endregion

		#region Private and protected

		private void RunService(ServiceController<IGaiaService> serviceController)
		{
			HostFactory.Run(x =>
			{
				//x.UseLinuxIfAvailable();
				x.ApplyCommandLine();

				x.StartAutomatically();
				x.RunAsNetworkService();
				x.EnableShutdown();
				x.EnableServiceRecovery(r => r.RestartService(2));

				x.SetDescription(Description);
				x.SetDisplayName(DisplayName);
				x.SetServiceName(ServiceName);

				x.EnablePauseAndContinue();
				x.EnableServiceRecovery(r => { r.RestartService(1); });

				x.Service(s => serviceController);
			});
		}

		/// <summary>
		///   Factory creates and run service
		/// </summary>
		/// <param name="serviceName">Service system name</param>
		/// <param name="displayName">Service display name</param>
		/// <param name="description">Service description</param>
		/// <param name="pluginsConfiguration"></param>
		/// <param name="wcfServicesConfiguration"></param>
		/// <returns></returns>
		public static ServiceFactory Create<T>(string serviceName, string displayName,
			string description = null, PluginConfigurationCollection pluginsConfiguration = null,
			ServiceHostConfigurationCollection wcfServicesConfiguration = null)
		{
			var serVact = new ServiceFactory
			{
				ServiceName = serviceName,
				DisplayName = displayName,
				Description = description
			};

		    var serviceController = new ServiceController<IGaiaService>(
				(IGaiaService) Activator.CreateInstance(typeof (T)),
				pluginsConfiguration, wcfServicesConfiguration);

			serVact.RunService(serviceController);
			return serVact;
		}

		/// <summary>
		///   Factory creates and run service
		/// </summary>
		/// <param name="pluginsConfiguration"></param>
		/// <param name="wcfServicesConfiguration"></param>
		/// <returns></returns>
		public static ServiceFactory Create<T>(PluginConfigurationCollection pluginsConfiguration = null,
			ServiceHostConfigurationCollection wcfServicesConfiguration = null)
		{
			var serviceType = typeof (T);

			var serviceName = serviceType.Name;
			var displayName = serviceType.FullName;
			var description =
				$"{serviceName}{Environment.NewLine}{displayName}{Environment.NewLine}{serviceType.Assembly.Location}";

			var info = serviceType.GetCustomAttribute<ServiceInfoAttribute>();
			if (info != null)
			{
				serviceName = info.ServiceName;
				displayName = info.DisplayName;
				description = info.Description;
			}

			return Create<T>(serviceName, displayName, description, pluginsConfiguration, wcfServicesConfiguration);
		}

		#endregion
	}
}