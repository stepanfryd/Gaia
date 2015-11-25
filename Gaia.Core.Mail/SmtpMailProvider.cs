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
			_smtpClient.Send(message);
		}

		#endregion

		#region Private and protected

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