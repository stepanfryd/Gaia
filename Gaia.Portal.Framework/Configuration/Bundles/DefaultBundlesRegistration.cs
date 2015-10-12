using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Gaia.Portal.Framework.Configuration.EntLib;
using Gaia.Portal.Framework.Configuration.Modules;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Bundles
{
	/// <summary>
	///   Common optimatization bundle configuration
	/// </summary>
	public class BundlesRegistration : IBundlesRegistration
	{
		#region Constructor

		/// <summary>
		///   Creates instance of Bundle config
		/// </summary>
		/// <param name="config">Instance of common configuration object</param>
		/// <param name="modulesProvider">Web module configuration provider</param>
		/// <param name="entLib">Instance of enterprise library wraper</param>
		public BundlesRegistration(IConfiguration config, IWebModuleProvider modulesProvider, IEnterpriseLibrary entLib)
		{
			_config = config;
			_modulesProvider = modulesProvider;
			_entLib = entLib;
		}

		#endregion

		#region Public methods

		/// <summary>
		///   Registrer bundles from configuration file
		/// </summary>
		/// <param name="bundles">Existing bundle collection</param>
		public void RegisterBundles(BundleCollection bundles)
		{
			_bundles = bundles;
			_entLib.ExceptionManager.Process(() =>
			{
				LoadConfiguration();
				RegisterCommonBundles();
				RegisterModulesBundles();
			}, Constants.ExceptionPolicy.SwallowUp);
		}

		#endregion

		#region Private members

		private readonly IConfiguration _config;
		private readonly IWebModuleProvider _modulesProvider;
		private readonly IEnterpriseLibrary _entLib;

		private BundleCollection _bundles;
		private BundlesConfig _bundlesConfig;

		#endregion

		#region Private methods

		private void LoadConfiguration()
		{
			var path = _config.ApplicationSettings.BundlesConfig.StartsWith("~")
				? HttpContext.Current.Server.MapPath(_config.ApplicationSettings.BundlesConfig)
				: _config.ApplicationSettings.BundlesConfig;
			_bundlesConfig = JsonConvert.DeserializeObject<BundlesConfig>(File.ReadAllText(path));
		}

		private void RegisterCommonBundles()
		{
			if (_bundlesConfig != null && _bundlesConfig.Bundles != null)
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

					_bundles.Add(bn);
				}
			}
		}

		private void RegisterModulesBundles()
		{
			if (_modulesProvider != null)
			{
				foreach (var bun in _modulesProvider.Modules.SelectMany(c => c.Bundles))
				{
					_bundles.Add(bun);
				}
			}
		}

		#endregion
	}
}