using Gaia.Core.Mail.Configuration;

namespace Gaia.Portal.Framework.Configuration
{
	public interface IConfiguration
	{
		IGaiaSettings ApplicationSettings { get; set; }
		IEmailSettings EmailSettings { get; set; }
	}
}