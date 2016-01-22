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
using Gaia.Core.Exceptions;
using Gaia.Core.Services.Configuration;

namespace Gaia.Core.Services
{
	/// <summary>
	///   Plugins manager is responsible for loading configured plugins
	/// </summary>
	[Serializable]
	public class PluginsManager : IDisposable
	{
		#region Constructors

		/// <summary>
		///   Base constructor of plugin manager
		/// </summary>
		/// <param name="plugins">Plugins configuration collection</param>
		public PluginsManager(PluginConfigurationCollection plugins)
		{
			_pluginsConfiguration = plugins;
			_plugins = new List<IServicePlugin>();
		}

		#endregion

		#region Private and protected
		/// <summary>
		/// Class destructor
		/// </summary>
		~PluginsManager()
		{
			Dispose(false);
		}

		#endregion

		#region Fields and constants

		private readonly List<IServicePlugin> _plugins;
		private readonly PluginConfigurationCollection _pluginsConfiguration;

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
				ShutdownPlugins();
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		///   Initialize all configured plugins
		/// </summary>
		public void InitalizePlugins()
		{
			if (_pluginsConfiguration == null || _pluginsConfiguration.Count == 0)
			{
				return;
			}

			foreach (var plugin in _pluginsConfiguration)
			{
				if (plugin.PluginType != null && typeof (IServicePlugin).IsAssignableFrom(plugin.PluginType))
				{
					var plutinInstance = Activator.CreateInstance(plugin.PluginType) as IServicePlugin;

					if (plutinInstance != null)
					{
						plutinInstance.Initialize();
						_plugins.Add(plutinInstance);
					}
				}
				else
				{
					throw new NotImplementedPluginInterfaceException(plugin.PluginTypeName);
				}
			}
		}

		//private void domain_DomainUnload(object sender, EventArgs e)
		//{
		//}

		/// <summary>
		///   Shutdown all services which are up and running
		/// </summary>
		public void ShutdownPlugins()
		{
			//if (_pluginDomains != null)
			//{
			//	foreach (AppDomain pluginDomain in _pluginDomains)
			//	{
			//		AppDomain.Unload(pluginDomain);
			//	}
			//}

			if (_plugins != null)
			{
				foreach (var plugin in _plugins)
				{
					plugin.Uninitialize();
					plugin.Dispose();
				}
			}
		}

		#endregion
	}
}