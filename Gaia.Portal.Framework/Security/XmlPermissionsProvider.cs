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