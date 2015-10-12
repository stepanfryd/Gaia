using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Common.Logging;
using Gaia.Portal.Framework.Bundling;
using Gaia.Portal.Framework.Configuration.EntLib;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Configuration.Modules
{
	public class DefaultWebModuleProvider : IWebModuleProvider
	{
		#region
		private const string MODULE_MASK = @"{Module}";
		private const string INCLUDE_FILE_MASK = @"{ModuleConfig}";
		private readonly IConfiguration _config;

		private readonly IEnterpriseLibrary _entLib;
		private readonly ILog _log;

		#endregion


		#region Public properties
		public IList<IWebModule> Modules { get; private set; }
		#endregion

		#region Constructors 

		public DefaultWebModuleProvider(IConfiguration configuration, IEnterpriseLibrary entLib)
		{
			_config = configuration;
			_entLib = entLib;
			_log = LogManager.GetLogger(GetType());

			if (Modules == null)
			{
				LoadModules();
			}
		}
		#endregion


		public void LoadModules()
		{
			Modules = new List<IWebModule>();

			var areasRootPath = _config.ApplicationSettings.ModulesRoot;
			var moduleScriptVirtualPath = _config.ApplicationSettings.ModuleIncludeMask.Replace(INCLUDE_FILE_MASK, "");
			var areasRootDir = new DirectoryInfo(HttpContext.Current.Server.MapPath(areasRootPath));

			if (areasRootDir.Exists)
			{
				foreach (var dir in areasRootDir.GetDirectories())
				{
					var includeVirtPath = _config.ApplicationSettings.ModuleIncludeMask.Replace(MODULE_MASK, dir.Name)
						.Replace(INCLUDE_FILE_MASK, _config.ApplicationSettings.ModuleConfigFile);
					var moduleConfigFile = new FileInfo(HttpContext.Current.Server.MapPath(includeVirtPath));

					if (moduleConfigFile.Exists)
					{
						_entLib.ExceptionManager.Process(delegate
						{
							var webModule =
								JsonConvert.DeserializeObject<DefaultWebModule>(File.ReadAllText(moduleConfigFile.FullName));

							if (!string.IsNullOrEmpty(webModule.IocContainerConfig))
							{
								var contFullPath =
									HttpContext.Current.Server.MapPath(
										$"{moduleScriptVirtualPath}{webModule.IocContainerConfig}".Replace(MODULE_MASK, dir.Name));
								if (File.Exists(contFullPath))
								{
									webModule.IocContainerFullPath = contFullPath;
								}
							}

							var modulesScriptBundle = new ScriptBundle(_config.ApplicationSettings.ModuleScriptBundleMask.Replace(MODULE_MASK, dir.Name))
							{
								Orderer = new NonOrderingBundleOrderer()
							};

							foreach (var p in webModule.Scripts
									.Select(p => $"{moduleScriptVirtualPath}{p}".Replace(MODULE_MASK, dir.Name)))
							{
								try
								{
									modulesScriptBundle.Include(p);
								}
								catch (ArgumentException e)
								{
									if (e.ParamName == "directoryVirtualPath")
									{
										_log.ErrorFormat("Directory for virtual path '{0}' doesn't exists", p);
									}
									else
									{
										throw;
									}
								}
							}

							webModule.Bundles.Add(modulesScriptBundle);
							Modules.Add(webModule);
						}, Constants.ExceptionPolicy.SwallowUp);

					}
					else
					{
						_log.ErrorFormat("Module configuration file doesn't exists. File '{0}'", moduleConfigFile.FullName);
					}
				}
			}

		}
	}
}
