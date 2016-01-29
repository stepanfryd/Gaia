using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace Gaia.Core.Mail
{
	/// <summary>
	///   Mail helpers and utilities
	/// </summary>
	public class Utils
	{
		#region Fields

		private static volatile Utils _instance;
		private static readonly object SyncRoot = new object();

		private readonly MethodInfo _closeMethod;
		private readonly ConstructorInfo _mailWriterContructor;
		private readonly MethodInfo _sendMethod;

		#endregion

		#region Constructors

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

		#region Public

		/// <summary>
		///   Singleton instance of Utils class
		/// </summary>
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

		/// <summary>
		///   Method returns email content as a byte array
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public byte[] GetMailBytes(MailMessage message)
		{
			byte[] retVal;

			using (var ms = new MemoryStream())
			{
				// Construct mail writer object
				var mailWriter = _mailWriterContructor.Invoke(new object[] {ms});

				// Call Send method
				_sendMethod?.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {mailWriter, true, true},
					null);

				retVal = ms.GetBuffer();

				// Close mail writer object
				_closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {}, null);
				ms.Close();
			}
			return retVal;
		}
	}
}