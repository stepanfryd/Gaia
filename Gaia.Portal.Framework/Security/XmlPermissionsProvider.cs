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
using System.Web;
using System.Xml.Serialization;

namespace Gaia.Portal.Framework.Security
{
	/// <summary>
	///   Permissoin provider reads roles and permissions for routes from xml source
	/// </summary>
	public class XmlPermissionsProvider : IPermissionsProvider
	{
		private static string _xmlConfigurationFilePath;

		private readonly Lazy<Permissions> _permissionLazy = new Lazy<Permissions>(() =>
		{
			var ser = new XmlSerializer(typeof(Permissions));
			return ser.Deserialize(new StringReader(File.ReadAllText(_xmlConfigurationFilePath))) as Permissions;
		}, true);

		/// <summary>
		///   Xml configuration permisison provider. Loads roles and route permissions from xml file
		/// </summary>
		/// <param name="filePath">Xml file which contains roles and routes configuration</param>
		public XmlPermissionsProvider(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath), "Xml configuration file for permission is not specified");

			_xmlConfigurationFilePath = filePath.StartsWith("~") ? HttpContext.Current.Server.MapPath(filePath) : filePath;

			if (!File.Exists(_xmlConfigurationFilePath))
			{
				throw new FileNotFoundException("XML configuration file doesn't exists", _xmlConfigurationFilePath);
			}
		}

		/// <summary>
		///   Method returns object with declared roles and routes to secure
		/// </summary>
		/// <returns></returns>
		public Permissions GetPermissions()
		{
			return _permissionLazy.Value;
		}
	}
}