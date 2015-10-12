namespace Gaia.Portal.Framework.Security
{
	/// <summary>
	///   Interface describes permission manager
	/// </summary>
	public interface IPermissionsProvider
	{
		/// <summary>
		///   Method returns object with declared roles and routes to secure
		/// </summary>
		/// <returns></returns>
		Permissions GetPermissions();
	}
}