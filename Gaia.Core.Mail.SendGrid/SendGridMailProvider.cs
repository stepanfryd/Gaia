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

using Gaia.Core.Mail.SendGrid.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Linq;

// using SendGridWeb = SendGrid.Web;

namespace Gaia.Core.Mail.SendGrid
{
	/// <summary>
	/// Mail provider for SendGrid service
	/// </summary>
	public class SendGridMailProvider : IMailProvider
	{
		#region Private Fields

		private readonly SendGridClient _sendGridClient;

		#endregion

		#region Public Constructors

		/// <summary>
		/// SendGrid default constructor
		/// </summary>
		/// <param name="sendGridSettings">
		/// </param>
		public SendGridMailProvider(SendGridSettings sendGridSettings = null)
		{
			var settings = sendGridSettings ?? new Settings().SendGrid;
			if (settings == null) throw new SendGridSettingsException();

			_sendGridClient = new SendGridClient(settings.ApiKey);
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Send mail message
		/// </summary>
		/// <param name="message">
		/// </param>
		/// <param name="objectId">
		/// </param>
		/// <param name="sendTime">
		/// </param>
		public async void Send(System.Net.Mail.MailMessage message, object objectId = null, DateTime? sendTime = null)
		{
			var sendGridMessage = new SendGridMessage
			{
				From = new EmailAddress(message.From.Address, message.From.DisplayName),
				Subject = message.Subject,
				PlainTextContent = message.Body
			};

			sendGridMessage.AddTos(message.To.Select(a => new EmailAddress(a.Address, a.DisplayName)).ToList());

			var stream = message.AlternateViews[0].ContentStream;
			using (var reader = new StreamReader(stream))
			{
				sendGridMessage.HtmlContent = reader.ReadToEnd();
			}

			foreach (var att in message.Attachments)
			{
				if (att.ContentStream != null && att.ContentStream.CanRead)
				{
					using (var ms = new MemoryStream())
					{
						await att.ContentStream.CopyToAsync(ms);
						sendGridMessage.AddAttachment(att.Name, Convert.ToBase64String(ms.ToArray()), att.ContentType.MediaType, att.ContentDisposition.DispositionType, att.ContentId);
						ms.Close();
					}
				}

				await _sendGridClient.SendEmailAsync(sendGridMessage);
			}

			#endregion Public Methods
		}
	}
}