using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Gaia.Core.EnterpriseLibrary.Logging
{
	[ConfigurationElementType(typeof (CustomTraceListenerData))]
	public class ConsoleTraceListener : CustomTraceListener
	{
		#region Private and protected

		public override void TraceData(TraceEventCache eventCache,
			string source, TraceEventType eventType, int id, object data)
		{
			var oldTextColor = Console.ForegroundColor;

			switch (eventType)
			{
				case TraceEventType.Critical:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case TraceEventType.Error:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case TraceEventType.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case TraceEventType.Information:
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case TraceEventType.Verbose:
					Console.ForegroundColor = ConsoleColor.Magenta;
					break;
			}

			if (data is LogEntry && Formatter != null)
			{
				var le = (LogEntry) data;
				WriteLine(Formatter.Format(le));
			}
			else
			{
				WriteLine(data.ToString());
			}

			Console.ForegroundColor = oldTextColor;
		}

		public override void Write(string message)
		{
			Console.Write(message);
		}

		public override void WriteLine(string message)
		{
			Console.WriteLine(message);
		}

		#endregion
	}
}