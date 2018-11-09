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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Gaia.Core.Services.Configuration;
using Gaia.Portal.Framework.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Gaia.Core.Tests
{
	[TestClass]
	public class ConfigurationTests
	{
		[TestMethod]
		public void PluginConfigurationSettingsSerializationTest()
		{
			var expectedXml =
				"<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<plugins xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
				"<plugin name=\"Plugin\" type=\"Test.TestPluginType\" />" +
				"<plugin name=\"Plugin1\" type=\"Test1.TestPluginType\" />" +
				"</plugins>";

			var config = new PluginConfigurationCollection
			{
				new PluginConfiguration
				{
					Name = "Plugin",
					PluginTypeName = "Test.TestPluginType"
				},
				new PluginConfiguration
				{
					Name = "Plugin1",
					PluginTypeName = "Test1.TestPluginType"
				}
			};

			string serData;
			var seri = new XmlSerializer(typeof (PluginConfigurationCollection));

			using (var ms = new MemoryStream())
			{
				using (var sr = new StreamWriter(ms))
				{
					using (var xmlWriter = XmlWriter.Create(sr, new XmlWriterSettings {Indent = false}))
					{
						seri.Serialize(xmlWriter, config);
						ms.Seek(0, SeekOrigin.Begin);
						serData = Encoding.Default.GetString(ms.GetBuffer());
						sr.Close();
					}
				}
				ms.Close();
			}

			Assert.AreEqual(expectedXml, serData);
			PluginConfigurationCollection expectedCollection;

			using (var ms = new MemoryStream(Encoding.Default.GetBytes(expectedXml)))
			{
				ms.Seek(0, SeekOrigin.Begin);
				expectedCollection = (PluginConfigurationCollection) seri.Deserialize(ms);
				ms.Close();
			}

			Assert.IsNotNull(expectedCollection);
			Assert.AreEqual(expectedCollection[0].Name, "Plugin");
			Assert.AreEqual(expectedCollection[0].PluginTypeName, "Test.TestPluginType");
			Assert.AreEqual(expectedCollection[1].Name, "Plugin1");
			Assert.AreEqual(expectedCollection[1].PluginTypeName, "Test1.TestPluginType");
		}

		[TestMethod]
		public void PortalConfigSettingsDeserializationTest()
		{
			var str =
				"<?xml version=\"1.0\"?><web xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanceq\" " +
				"apiDependencyResolver=\"Gaia.Portal.Framework.Constants, Gaia.Portal.Framework\" " +
				"mvcDependencyResolver=\"Gaia.Portal.Framework.Constants, Gaia.Portal.Framework\">" +
				"<FilterProviders><filterProvider>Gaia.Portal.Framework.Constants, Gaia.Portal.Framework</filterProvider>" +
				"<filterProvider>Gaia.Portal.Framework.Constants, Gaia.Portal.Framework</filterProvider></FilterProviders></web>";


			var ser = new XmlSerializer(typeof (WebSettings));
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
			{
				ms.Seek(0, SeekOrigin.Begin);
				var obj = ser.Deserialize(ms);
				ms.Close();
			}
		}

		[TestMethod]
		public void PortalConfigurationSettingsSerializationTest()
		{
			var settings = new WebSettings
			{
				ApiDependencyResolver = "Gaia.Portal.Framework.Constants, Gaia.Portal.Framework",
				MvcDependencyResolver = "Gaia.Portal.Framework.Constants, Gaia.Portal.Framework",
				FilterProviders = new List<string>
				{
					"Gaia.Portal.Framework.Constants, Gaia.Portal.Framework",
					"Gaia.Portal.Framework.Constants, Gaia.Portal.Framework"
				}
			};

			var ser = new XmlSerializer(typeof (WebSettings));

			byte[] data;
			using (var ms = new MemoryStream())
			{
				ser.Serialize(ms, settings);
				data = ms.GetBuffer();
				ms.Close();
			}

			var str = Encoding.UTF8.GetString(data);
		}

		[TestMethod]
		public void PortalConfiguraitonTest()
		{
			var test = new Gaia.Portal.Framework.Configuration.Configuration();
			

		}
	}
}