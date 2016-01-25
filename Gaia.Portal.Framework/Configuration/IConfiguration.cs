using Gaia.Core.Mail.Configuration;

namespace Gaia.Portal.Framework.Configuration
{
	public interface IConfiguration
	{
		GaiaSettings ApplicationSettings { get; set; }
		EmailSettings EmailSettings { get; set; }
	}
}