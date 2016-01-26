using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace Gaia.Core.Mail
{
	public class Utils
	{
		private static volatile Utils _instance;
		private static readonly object SyncRoot = new object();

		private readonly MethodInfo _closeMethod;
		private readonly ConstructorInfo _mailWriterContructor;
		private readonly MethodInfo _sendMethod;

		#region Public properties

		public static Utils Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new Utils();
					}
				}

				return _instance;
			}
		}

		#endregion

		#region Constructor

		private Utils()
		{
			var smtpClientAssembly = typeof (SmtpClient).Assembly;
			var mailWriterType = smtpClientAssembly.GetType("System.Net.Mail.MailWriter");
			_mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
				new[] {typeof (Stream)}, null);
			_sendMethod = typeof (MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
			_closeMethod = mailWriterType.GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		#endregion

		#region Public methods

		public byte[] GetMailBytes(MailMessage message)
		{
			byte[] retVal;

			using (var ms = new MemoryStream())
			{
				// Construct
				var mailWriter = _mailWriterContructor.Invoke(new object[] {ms});

				// Send
				_sendMethod?.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {mailWriter, true, true},
					null);

				retVal = ms.GetBuffer();

				// Close
				_closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {}, null);
				ms.Close();
			}
			return retVal;
		}

		#endregion
	}
}