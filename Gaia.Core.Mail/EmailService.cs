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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Gaia.Core.Mail.Configuration;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MimeMapping;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Gaia.Core.Mail
{
	/// <summary>
	///   Emails service is responsible for sending emails following system configuration and use razor templating
	/// </summary>
	public class EmailService : IEmailService
	{
		#region Fields and constants

		private readonly IEmailSettings _emailSettings;
		private readonly IEmailTemplateConfiguration _emailTemplateConfiguration;
		private readonly IRazorEngineService _engineService;
		private readonly ILogger _logger;
		private readonly IMailProvider _mailProvider;
		private readonly IMessageTemplateProvider _templateProvider;

		#endregion Fields and constants

		#region Constructors

		/// <summary>
		///   Create instance of EmailService with specified providers
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="mailProvider"></param>
		/// <param name="templateProvider"></param>
		/// <param name="emailTemplateConfiguration"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="EmailSettingsException"></exception>
		public EmailService(ILogger<EmailService> logger, IMailProvider mailProvider,
			IMessageTemplateProvider templateProvider, IEmailTemplateConfiguration emailTemplateConfiguration)
		{
			_logger = logger;
			_emailSettings = new Settings().EmailSettings;

			if (_emailSettings == null)
			{
				throw new EmailSettingsException();
			}

			_mailProvider = mailProvider ?? throw new ArgumentNullException(nameof(mailProvider));
			_templateProvider = templateProvider;
			_emailTemplateConfiguration = emailTemplateConfiguration ??
			                              throw new ArgumentNullException(nameof(emailTemplateConfiguration));
			_engineService = RazorEngineService.Create((ITemplateServiceConfiguration) _emailTemplateConfiguration);
		}

		#endregion Constructors

		#region Interface Implementations

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(MailAddress sender, MailAddress[] recipients, string templateKey, T model,
			Attachment[] attachments = null, IDictionary<string, string> customHeaders = null)
		{
			var messageTemplate = _templateProvider.GetTemplate(templateKey);
			await SendEmailAsync(sender, recipients, messageTemplate.Subject, messageTemplate.TemplatePlain,
				messageTemplate.TemplateHtml, model, templateKey, attachments, customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(string sender, string[] recipients, string templateKey, T model,
			Attachment[] attachments = null, IDictionary<string, string> customHeaders = null)
		{
			await SendEmailAsync(new MailAddress(sender), recipients.Select(r => new MailAddress(r)).ToArray(), templateKey,
				model,
				attachments, customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(string sender, string recipient, string templateKey, T model,
			Attachment[] attachments = null, IDictionary<string, string> customHeaders = null)
		{
			await SendEmailAsync(new MailAddress(sender), new[] {new MailAddress(recipient)}, templateKey, model, attachments,
				customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <param name="subject"></param>
		/// <param name="templatePlain"></param>
		/// <param name="templateHtml"></param>
		/// <param name="model"></param>
		/// <param name="templateKey"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(MailAddress sender, MailAddress recipient, string subject, string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null)
		{
			await SendEmailAsync(sender, new[] {recipient}, subject, templatePlain, templateHtml, model, templateKey,
				attachments, customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <param name="subject"></param>
		/// <param name="templatePlain"></param>
		/// <param name="templateHtml"></param>
		/// <param name="model"></param>
		/// <param name="templateKey"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(string sender, string recipient, string subject, string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null)
		{
			await SendEmailAsync(new MailAddress(sender), new MailAddress(recipient), subject, templatePlain, templateHtml,
				model, templateKey, attachments, customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="subject"></param>
		/// <param name="templatePlain"></param>
		/// <param name="templateHtml"></param>
		/// <param name="model"></param>
		/// <param name="templateKey"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(string sender, string[] recipients, string subject, string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null)
		{
			await SendEmailAsync(new MailAddress(sender), recipients.Select(r => new MailAddress(r)).ToArray(), subject,
				templatePlain, templateHtml, model, templateKey, attachments, customHeaders);
		}

		/// <summary>
		///   Send email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="subject"></param>
		/// <param name="templatePlain"></param>
		/// <param name="templateHtml"></param>
		/// <param name="model"></param>
		/// <param name="templateKey"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public async Task SendEmailAsync<T>(MailAddress sender, MailAddress[] recipients, string subject,
			string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null)
		{
			if (string.IsNullOrEmpty(sender?.Address))
			{
				throw new ArgumentNullException(nameof(sender));
			}

			if (recipients == null || !recipients.Any())
			{
				throw new ArgumentNullException(nameof(recipients));
			}

			if (string.IsNullOrEmpty(subject))
			{
				throw new ArgumentNullException(nameof(subject));
			}

			if (string.IsNullOrEmpty(templatePlain))
			{
				throw new ArgumentNullException(nameof(templatePlain));
			}

			var modelType = typeof(T);

			var templateContentPlain = _engineService.IsTemplateCached(templateKey + "Plain", typeof(T))
				? _engineService.Run(templateKey + "Plain", typeof(T), model)
				: _engineService.RunCompile(templatePlain, templateKey + "Plain", typeof(T), model);

			var subjectProperty = modelType.GetProperties().SingleOrDefault(p => p.Name.ToLower() == "subject");
			if (subjectProperty != null && subjectProperty.PropertyType == typeof(string))
			{
				if (subjectProperty.GetValue(model) is string sVal)
				{
					subject = sVal;
				}
			}

			var message = new MailMessage
			{
				SubjectEncoding = Encoding.UTF8,
				BodyEncoding = Encoding.UTF8,
				From = sender,
				Subject = subject,
				Body = templateContentPlain,
				IsBodyHtml = false
			};

			if (customHeaders != null)
			{
				foreach (var ch in customHeaders.Where(ch => !message.Headers.AllKeys.Contains(ch.Key)))
				{
					message.Headers.Add(ch.Key, ch.Value);
				}
			}

			if (!string.IsNullOrEmpty(templateHtml))
			{
				var templateContentHtml = _engineService.IsTemplateCached(templateKey + "Html", typeof(T))
					? _engineService.Run(templateKey + "Html", typeof(T), model)
					: _engineService.RunCompile(templateHtml, templateKey + "Html", typeof(T), model);

				templateContentHtml = EmbedImages(templateContentHtml, out var embeddedImages);

				var htmlView = AlternateView.CreateAlternateViewFromString(templateContentHtml);
				htmlView.ContentType = new ContentType("text/html")
				{
					CharSet = "utf8"
				};

				foreach (var lr in embeddedImages)
				{
					htmlView.LinkedResources.Add(lr);
				}

				message.AlternateViews.Add(htmlView);
			}

			if (_emailSettings.TestMode)
			{
				message.To.Add(_emailSettings.TestRecipient);
			}
			else
			{
				foreach (var r in recipients)
				{
					message.To.Add(r);
				}
			}

			if (attachments != null)
			{
				foreach (var att in attachments)
				{
					message.Attachments.Add(att);
				}
			}

			if (_emailSettings.SaveCopy
			    && !string.IsNullOrEmpty(_emailSettings.CopyLocation)
			    && Directory.Exists(_emailSettings.CopyLocation))
			{
				var client = new SmtpClient
				{
					DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
					PickupDirectoryLocation = _emailSettings.CopyLocation
				};
				client.Send(message);
			}

			if (_emailSettings.Enabled)
			{
				await _mailProvider.SendAsync(message);
			}
		}

		#endregion Interface Implementations

		#region Private and protected

		private string EmbedImages(string source, out List<LinkedResource> embeddedImages)
		{
			embeddedImages = new List<LinkedResource>();
			var doc = new HtmlDocument();
			doc.LoadHtml(source);

			var images = doc.DocumentNode.SelectNodes("//img");

			if (images != null)
			{
				foreach (var img in images)
				{
					try
					{
						var srcAttr = img.Attributes.FirstOrDefault(a => a.Name.ToLower() == "src");
						if (srcAttr != null)
						{
							if (srcAttr.Value.ToLower().StartsWith("data:"))
							{
								// data:image/png;base64,
								var p1 = srcAttr.Value.Split(',');

								var lr = new LinkedResource(
									new MemoryStream(
										Convert.FromBase64String(p1[1])),
									p1[0].Split(';')[0].Split(':')[1]
								);
								embeddedImages.Add(lr);
								img.SetAttributeValue("src", $"cid:{lr.ContentId}");
							}
							else
							{
								var imgFullPath = Path.Combine(_emailTemplateConfiguration.TemplateFolder, srcAttr.Value);
								if (File.Exists(imgFullPath))
								{
									var mime = MimeUtility.GetMimeMapping(imgFullPath);
									var lr = new LinkedResource(imgFullPath, mime);
									embeddedImages.Add(lr);
									img.SetAttributeValue("src", $"cid:{lr.ContentId}");
								}
							}
						}
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Error embending image {img.Name}");
					}
				}
			}

			return doc.DocumentNode.OuterHtml;
		}

		#endregion Private and protected
	}
}