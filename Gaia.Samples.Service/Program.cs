using Gaia.Core.Services;
using Gaia.Samples.Service.Properties;

namespace Gaia.Samples.Service
{
	internal class Program
	{
		#region Private and protected

		private static void Main(string[] args)
		{
			ServiceFactory.Create<ServiceController>(
				Settings.Default.PluginsConfiguration,
				Settings.Default.WcfServicesConfiguration
				);
		}

		#endregion
	}
}