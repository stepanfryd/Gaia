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
using System.IO;
using Newtonsoft.Json;

namespace Gaia.Core.Mail
{
	/// <summary>
	///   Provides properties for Message template
	/// </summary>
	public class MessageTemplate : IMessageTemplate
	{
		#region Private and protected

		#region Private methods

		private string GetTemplate(string path, string templatePropertyName, string templatePathPropertyName)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException("Template file doesn't exists", path);
			}

			var retval = File.ReadAllText(path);

			if (string.IsNullOrEmpty(retval))
			{
				throw new ArgumentNullException("Template is not specified. " +
				                                $"Please set [{templatePropertyName}] property or [{templatePathPropertyName}] (relative or physical) " +
				                                "to template file");
			}

			return retval;
		}

		#endregion

		#endregion

		#region Private members

		private string _htmlTemplate;
		private string _textTemplate;

		#endregion

		#region Public properties

		/// <summary>
		///   Path to HTML template location
		/// </summary>
		[JsonProperty("templateHtmlPath")]
		public string TemplateHtmlPath { get; set; }

		/// <summary>
		///   List of indentificators which can spedify where is the template used. Eg. site, resource, server etc.
		/// </summary>
		[JsonProperty("targets")]
		public string[] Targets { get; set; }

		/// <summary>
		///   Path to plain text template location
		/// </summary>
		[JsonProperty("templatePlainPath")]
		public string TemplatePlainPath { get; set; }

		/// <summary>
		///   Message subject
		/// </summary>
		[JsonProperty("subject")]
		public string Subject { get; set; }

		/// <summary>
		///   HTML content of message template
		/// </summary>
		[JsonProperty("htmlTemplate")]
		public string TemplateHtml
		{
			get
			{
				var retval = _htmlTemplate;

				if (string.IsNullOrEmpty(TemplateHtmlPath))
				{
					if (!string.IsNullOrEmpty(_htmlTemplate))
					{
						retval = _htmlTemplate;
					}
				}
				else
				{
					retval = GetTemplate(TemplateHtmlPath, nameof(TemplateHtml), nameof(TemplateHtmlPath));
				}

				return retval;
			}
			set => _htmlTemplate = value;
		}

		/// <summary>
		///   Plain text content of template
		/// </summary>
		public string TemplatePlain
		{
			get
			{
				var retval = _textTemplate;

				if (string.IsNullOrEmpty(TemplatePlainPath))
				{
					if (!string.IsNullOrEmpty(_textTemplate))
					{
						retval = _textTemplate;
					}
				}
				else
				{
					retval = GetTemplate(TemplatePlainPath, nameof(TemplatePlain), nameof(TemplatePlainPath));
				}

				return retval;
			}

			set => _textTemplate = value;
		}

		#endregion
	}
}