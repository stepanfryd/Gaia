/*
The MIT License (MIT)

Copyright (c) 2012 Stepan Fryd (stepan.fryd@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using Common.Logging;

namespace Gaia.Core.Mail
{
	public class SmtpMailProvider : IMailProvider, IDisposable
	{
		#region Fields and constants

		private readonly ILog _log;
		private SmtpClient _smtpClient;

		#endregion

		#region Constructors

		public SmtpMailProvider()
		{
			_log = LogManager.GetLogger(GetType());
			_smtpClient = new SmtpClient();
			_smtpClient.SendCompleted += smtpClient_SendCompleted;
		}

		#endregion

		#region Interface Implementations

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Send(MailMessage message)
		{
			_smtpClient.SendAsync(message, message);
		}

		#endregion

		#region Private and protected

		private void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
		{
			var mail = e.UserState as MailMessage;


			if (mail != null)
			{
				var mailHeaders = mail.Headers.ToJson();
				var recipients = string.Join(",", mail.To.Select(m => m.Address));
				if (e.Cancelled)
				{
					_log.Warn($"Email delivery to {recipients} has been canceled. Headers: {mailHeaders}");
				}

				if (e.Error != null)
				{
					_log.ErrorFormat("Email to {0} delivery error. Headers: {1}", e.Error, recipients, mailHeaders);
				}
				else
				{
					_log.Warn($"Email has been sent to {recipients}. Headers: {mailHeaders}");
				}
			}
			else
			{
				_log.Warn("Email token after mail complete doesn't exists");
			}
		}

		~SmtpMailProvider()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_smtpClient != null)
				{
					_smtpClient.Dispose();
					_smtpClient = null;
				}
			}
		}

		#endregion
	}
}