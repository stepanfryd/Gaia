using System;
using System.Runtime.Serialization;
using Gaia.Core.Exceptions;

namespace Gaia.Portal.Framework.Exceptions
{
	public class PortalBaseException : GaiaBaseException
	{
		public PortalBaseException(string message) : base(message) {}

		public PortalBaseException(string message, Exception innerException) : base(message, innerException) {}

		public PortalBaseException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}