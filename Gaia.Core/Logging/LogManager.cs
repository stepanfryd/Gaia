using System;

namespace Gaia.Core.Logging
{
	public class LogManager
	{
		public static ILog GetLogger(Type type)
		{
			return LogProvider.GetLogger(type);
		}

		public static ILog GetLogger(string name)
		{
			return LogProvider.GetLogger(name);
		}

		public static ILog GetLogger<T>()
		{
			return LogProvider.For<T>();
		}
	}
}