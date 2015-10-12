using System;

namespace Gaia.Portal.Framework.Exceptions
{
	public class DependencyContainerLoadingException : PortalBaseException
	{
		public DependencyContainerLoadingException(string message) : base(message) {}

		public DependencyContainerLoadingException(string message, Exception innerException) : base(message, innerException) {}
	}
}