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

using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gaia.Core.Mail
{
	/// <summary>
	///   Describes email service interface
	/// </summary>
	public interface IEmailService
	{
		#region Private and protected

		/// <summary>
		///   Send email asynchronously
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
		Task SendEmailAsync<T>(MailAddress sender, MailAddress[] recipients, string subject, string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task SendEmailAsync<T>(MailAddress sender, MailAddress[] recipients, string templateKey, T model,
			Attachment[] attachments = null, IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipients"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task SendEmailAsync<T>(string sender, string[] recipients, string templateKey, T model,
			Attachment[] attachments = null, IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <param name="templateKey"></param>
		/// <param name="model"></param>
		/// <param name="attachments"></param>
		/// <param name="customHeaders"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task SendEmailAsync<T>(string sender, string recipient, string templateKey, T model, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
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
		Task SendEmailAsync<T>(MailAddress sender, MailAddress recipient, string subject, string templatePlain,
			string templateHtml, T model, string templateKey, Attachment[] attachments = null,
			IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
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
		Task SendEmailAsync<T>(string sender, string recipient, string subject, string templatePlain, string templateHtml,
			T model, string templateKey, Attachment[] attachments = null, IDictionary<string, string> customHeaders = null);

		/// <summary>
		///   Send email asynchronously
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
		Task SendEmailAsync<T>(string sender, string[] recipients, string subject, string templatePlain, string templateHtml,
			T model, string templateKey, Attachment[] attachments = null, IDictionary<string, string> customHeaders = null);

		#endregion Private and protected
	}
}