namespace Gaia.Core.Mail.Configuration
{
	public interface ISmtpClientSettings
	{
		string Host { get; set; }
		int Port { get; set; }
		bool EnableSsl { get; set; }
		ICredentials Credentials { get; set; }
	}
}