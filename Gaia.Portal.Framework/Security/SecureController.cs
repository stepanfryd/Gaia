using Gaia.Portal.Framework.Web;

namespace Gaia.Portal.Framework.Security
{
	/// <summary>
	/// Application controller requires authorized access
	/// </summary>
	[GaiaAuthorize]
	public class SecureController : BaseController
	{
	}
}