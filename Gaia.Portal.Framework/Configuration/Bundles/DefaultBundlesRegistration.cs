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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Gaia.Core.Logging;
using Gaia.Portal.Framework.Configuration.Modules;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Bundles
{
	/// <summary>
	///   Common optimatization bundle configuration
	/// </summary>
	public class BundlesRegistration : IBundlesRegistration
	{
		#region Fields and constants

		private readonly ILog _log;

		#endregion

		#region Constructor

		/// <summary>
		///   Creates instance of Bundle config
		/// </summary>
		/// <param name="config">Instance of common configuration object</param>
		/// <param name="modulesProvider">Web module configuration provider</param>
		/// <param name="entLib">Instance of enterprise library wraper</param>
		public BundlesRegistration(IConfiguration config, IWebModuleProvider modulesProvider)
		{
			_log = LogManager.GetLogger(GetType());
			_config = config;
			_modulesProvider = modulesProvider;
		}

		#endregion

		#region Interface Implementations

		#region Public methods

		/// <summary>
		///   Registrer bundles from configuration file
		/// </summary>
		/// <param name="bundles">Existing bundle collection</param>
		public void RegisterBundles(BundleCollection bundles)
		{
			_bundles = bundles;

			try
			{
				LoadConfiguration();
				RegisterCommonBundles();
				RegisterModulesBundles();
			} catch (Exception e)	{
				_log.Error(e, "Bundles registration error");
				throw new Core.Exceptions.GaiaBaseException("Bundles registration error", e);
			}
		}

		#endregion

		#endregion

		#region Private members

		private readonly IConfiguration _config;
		private readonly IWebModuleProvider _modulesProvider;

		private BundleCollection _bundles;
		private BundlesConfig _bundlesConfig;

		#endregion

		#region Private methods

		private void LoadConfiguration()
		{
			var path = _config.ApplicationSettings.BundlesConfig.StartsWith("~")
				? HttpContext.Current.Server.MapPath(_config.ApplicationSettings.BundlesConfig)
				: _config.ApplicationSettings.BundlesConfig;

			var bundleData = File.ReadAllText(path);
			_log.InfoFormat("Loading bundles config form {0} => {1} => {2}", _config.ApplicationSettings.BundlesConfig, path,
				bundleData);

			_bundlesConfig = JsonConvert.DeserializeObject<BundlesConfig>(bundleData);
		}

		private void RegisterCommonBundles()
		{
			if (_bundlesConfig?.Bundles != null)
			{
				foreach (var bundle in _bundlesConfig.Bundles)
				{
					System.Web.Optimization.Bundle bn;
					switch (bundle.Type)
					{
						case BundleType.Script:
							bn = new ScriptBundle(bundle.Path);
							break;
						case BundleType.Style:
							bn = new StyleBundle(bundle.Path);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					bn.Include(bundle.Includes.ToArray());
					if (bundle.Orderer != null)
					{
						bn.Orderer = bundle.Orderer;
					}

					_log.InfoFormat("Registering bundle: {0}", bn.Path);

					_bundles.Add(bn);
				}
			}
			else
			{
				_log.Warn("Bundles config is null");
			}
		}

		private void RegisterModulesBundles()
		{
			if (_modulesProvider != null)
			{
				foreach (var bun in _modulesProvider.Modules.SelectMany(c => c.Bundles))
				{
					_log.InfoFormat("Registering module bundle: {0}", bun.Path);
					_bundles.Add(bun);
				}
			} else
			{
				_log.Warn("Modules provider is not implemented");
			}
		}

		#endregion
	}
}