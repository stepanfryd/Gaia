using System.Threading.Tasks;

namespace Gaia.Core.Mail
{
	public interface IMessageProvider
	{
		Task<string> GetCompiledMessageAsync<T>(string templateKey, string messageTemplate, T model);
	}
}