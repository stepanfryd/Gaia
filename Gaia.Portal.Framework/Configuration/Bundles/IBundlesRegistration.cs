using System.Web.Optimization;

namespace Gaia.Portal.Framework.Configuration.Bundles
{
	public interface IBundlesRegistration
	{
		/// <summary>
		///   Registrer bundles from configuration file
		/// </summary>
		void RegisterBundles(BundleCollection bundles);
	}
}