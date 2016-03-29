using Gaia.Core.EnterpriseLibrary;
using Gaia.Core.Services;

namespace Gaia.Samples.Service
{
	[ServiceInfo("Gaia.Samples.Service", "Gaia Samples Service", "Gaia Samples Service")]
	public class ServiceController : IGaiaService
	{
		#region Constructors

		public ServiceController()
		{
			Bootstrap.Initialize();
		}

		#endregion

		#region Interface Implementations

		public void Start()
		{
		}

		public void Stop()
		{
		}

		public void Continue()
		{
		}

		public void CustomCommandReceived()
		{
		}

		public void Pause()
		{
		}

		public void Shutdown()
		{
		}

		#endregion
	}
}