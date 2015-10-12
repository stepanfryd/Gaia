using System.Collections.Generic;

namespace Gaia.Portal.Framework.Configuration.Modules
{
	public interface IWebModuleProvider
	{
		IList<IWebModule> Modules { get;}
	}
}