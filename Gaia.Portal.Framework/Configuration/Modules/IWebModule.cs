using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Gaia.Portal.Framework.Configuration.Modules
{
	public interface IWebModule
	{
		string Name { get; set; }
		string AreaName { get; set; }
		string IocContainerConfig { get; set; }
		string IocContainerFullPath { get; set; }
		List<string> Scripts { get; set; }
		BundleCollection Bundles { get; set; }
	}
}