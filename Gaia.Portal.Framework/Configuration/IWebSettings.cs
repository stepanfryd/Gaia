using System.Collections.Generic;

namespace Gaia.Portal.Framework.Configuration
{
	public interface IWebSettings
	{
		#region  Public members

		string ApiDependencyResolver { get; set; }

		string MvcDependencyResolver { get; set; }

		List<string> FilterProviders { get; set; }

		#endregion
	}
}