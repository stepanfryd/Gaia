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
using System.IO;
using Newtonsoft.Json;

namespace Gaia.Core.Mail
{
	/// <summary>
	///   Template provider for template file store
	/// </summary>
	public class FileTemplateProvider : IMessageTemplateProvider
	{
		#region Public members

		/// <summary>
		///   Dictionarly of loaded templates
		/// </summary>
		public IDictionary<string, IMessageTemplate> Templates { get; }

		#endregion

		#region Constructors

		/// <summary>
		///   Initializes a new instance of the FileTemplateProvider class, which acts as a wrapper for file template message
		///   store.
		/// </summary>
		/// <param name="templatesConfigurationFile"></param>
		public FileTemplateProvider(string templatesConfigurationFile)
		{
	
			if (!File.Exists(templatesConfigurationFile))
				throw new FileNotFoundException("Templates configuration file doesn't exists", templatesConfigurationFile);

			Templates = new Dictionary<string, IMessageTemplate>();
			foreach (var pair in JsonConvert.DeserializeObject<Dictionary<string, MessageTemplate>>(
				File.ReadAllText(templatesConfigurationFile)))
			{
				Templates.Add(pair.Key, pair.Value);
			}
		}

		#endregion

		#region Interface Implementations

		/// <summary>
		///   Returns template from existing templates dictionary
		/// </summary>
		/// <param name="templateKey"></param>
		/// <returns></returns>
		public IMessageTemplate GetTemplate(string templateKey)
		{
			IMessageTemplate template;
			return Templates.TryGetValue(templateKey, out template) ? template : null;
		}

		#endregion
	}
}