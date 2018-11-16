namespace Gaia.Core.Mail.Configuration
{
	public interface ICredentials
	{
		string UserName { get; set; }
		string Password { get; set; }
	}
}