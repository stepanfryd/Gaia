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
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Configuration
{
	/// <summary>
	/// Common application settings usually in web.config under gaia/settings
	/// </summary>
	[Serializable]
	[XmlRoot("settings")]
	public class GaiaSettings : IGaiaSettings
	{
		/// <summary>
		/// Application name
		/// </summary>
		[XmlElement("applicationName")]
		public string ApplicationName { get; set; }

		/// <summary>
		/// Modules location (usualy ~/Areas)
		/// </summary>
		[XmlElement("modulesRoot")]
		public string ModulesRoot { get; set; }

		/// <summary>
		/// Mask used for module includes defined in configuration file
		/// </summary>
		[XmlElement("moduleIncludeMask")]
		public string ModuleIncludeMask { get; set; }

		/// <summary>
		/// Name of JSON configuration file (module.json)
		/// </summary>
		[XmlElement("moduleConfigFile")]
		public string ModuleConfigFile { get; set; }

		/// <summary>
		/// Default mask for script module bundles
		/// </summary>
		[XmlElement("moduleScriptBundleMask")]
		public string ModuleScriptBundleMask { get; set; }

		/// <summary>
		/// Default mask for style module bundles
		/// </summary>
		[XmlElement("moduleStylesBundleMask")]
		public string ModuleStylesBundleMask { get; set; }

		/// <summary>
		/// Path to common application bundles configuration JSON file
		/// </summary>
		[XmlElement("bundlesConfig")]
		public string BundlesConfig { get; set; }
	}
}