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

namespace Gaia.Core.Mail.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class EmailTemplateConfiguration : IEmailTemplateConfiguration
	{
		#region Public members

		/// <summary>
		/// Location of email templates. Can by physical or virtual
		/// </summary>
		public string TemplateFolder { get; }

		#endregion

		#region Constructors
		/// <summary>
		/// Default configuration constructor
		/// </summary>
		/// <param name="emailsTemplatesFolder"></param>
		public EmailTemplateConfiguration(string emailsTemplatesFolder)
		{
			if (string.IsNullOrEmpty(emailsTemplatesFolder))
			{
				var directoryInfo = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
				if (directoryInfo != null)
					TemplateFolder = directoryInfo.FullName;
			}
			else
			{
				TemplateFolder = emailsTemplatesFolder;
			}

			if (!Directory.Exists(TemplateFolder))
				throw new DirectoryNotFoundException($"Template directory {TemplateFolder} doesn't exists.");
		}

		#endregion
	}
}