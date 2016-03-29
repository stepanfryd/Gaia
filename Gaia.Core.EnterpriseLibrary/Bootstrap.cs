using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Gaia.Core.EnterpriseLibrary
{
	public class Bootstrap
	{
		#region Private and protected

		public static void Initialize()
		{
			Logger.SetLogWriter(new LogWriterFactory(ConfigurationSourceFactory.Create()).Create(), false);
		}

		#endregion
	}
}