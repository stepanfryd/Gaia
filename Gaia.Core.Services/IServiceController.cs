namespace Gaia.Core.Services
{
	/// <summary>
	/// Interface describes service controller classes
	/// </summary>
	public interface IServiceController
	{
		/// <summary>
		/// Controlled service
		/// </summary>
		IGaiaService Service { get; }

		/// <summary>
		/// Instance of service plugin manager if exists
		/// </summary>
		PluginsManager PluginsManager { get; }
	}
}