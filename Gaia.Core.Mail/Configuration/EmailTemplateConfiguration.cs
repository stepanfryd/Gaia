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
using System.IO;
using System.Reflection;
using System.Web;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Gaia.Core.Mail.Configuration
{
	public class EmailTemplateConfiguration : TemplateServiceConfiguration, IEmailTemplateConfiguration
	{
		#region Public members

		public string PhysicalEmailsTemplateFolder { get; }

		#endregion

		#region Constructors

		public EmailTemplateConfiguration(string emailsTemplatesFolder)
		{
			if (string.IsNullOrEmpty(emailsTemplatesFolder))
			{
				var directoryInfo = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
				if (directoryInfo != null)
					PhysicalEmailsTemplateFolder = directoryInfo.FullName;
			}
			else
			{
				PhysicalEmailsTemplateFolder = emailsTemplatesFolder.StartsWith("~/")
					? (HttpContext.Current != null
						? HttpContext.Current.Server.MapPath(emailsTemplatesFolder)
						: Path.Combine(Assembly.GetEntryAssembly().Location, emailsTemplatesFolder.TrimStart('~', '/')))
					: emailsTemplatesFolder;
			}

			if (!Directory.Exists(PhysicalEmailsTemplateFolder))
				throw new DirectoryNotFoundException($"Template directory {PhysicalEmailsTemplateFolder} doesn't exists.");

			TemplateManager = new DelegateTemplateManager(name =>
			{
				var templatePath = Path.Combine(PhysicalEmailsTemplateFolder, name);
				using (var reader = new StreamReader(templatePath))
				{
					return reader.ReadToEnd();
				}
			});
		}

		#endregion
	}
}