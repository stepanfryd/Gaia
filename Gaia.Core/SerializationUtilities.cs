using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gaia.Core
{
	/// <summary>
	///   Serialization and deserialization utilities for different formats
	/// </summary>
	public static class SerializationUtilities
	{
		#region Public

		/// <summary>
		///   Serialize object to XML string
		/// </summary>
		/// <param name="data"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static string SerializeXml<T>(this T data)
		{
			string retVal;
			var seri = new XmlSerializer(typeof(T));

			using (var ms = new MemoryStream())
			{
				using (var sr = new StreamWriter(ms))
				{
					using (var xmlWriter = XmlWriter.Create(sr, new XmlWriterSettings {Indent = false}))
					{
						seri.Serialize(xmlWriter, data);
					}
					ms.Seek(0, SeekOrigin.Begin);
					retVal = Encoding.Default.GetString(ms.ToArray());
					sr.Close();
				}
				ms.Close();
			}
			return retVal;
		}

		/// <summary>
		///   Deserialize from XML
		/// </summary>
		/// <param name="data"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T DeserialiyeXml<T>(string data)
		{
			T retVal;
			var seri = new XmlSerializer(typeof(T));

			using (var ms = new MemoryStream(Encoding.Default.GetBytes(data)))
			{
				ms.Seek(0, SeekOrigin.Begin);
				retVal = (T) seri.Deserialize(ms);
				ms.Close();
			}
			return retVal;
		}

		#endregion
	}
}