using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Gaia.Portal.Framework.Configuration.EntLib
{
	public interface IEnterpriseLibrary
	{
		ExceptionPolicyFactory ExceptionFactory { get; }
		LogWriterFactory LogWriterFactory { get; }
		ExceptionManager ExceptionManager { get; }
	}
}