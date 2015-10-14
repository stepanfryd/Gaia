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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gaia.Portal.Framework.Json
{
	public class JsonNetValueProviderFactory : ValueProviderFactory
	{
		#region Private and protected

		public override IValueProvider GetValueProvider(ControllerContext controllerContext)
		{
			if (controllerContext == null)
				throw new ArgumentNullException(nameof(controllerContext));

			if (
				!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
				return null;

			var streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
			var jsonReader = new JsonTextReader(streamReader);
			if (!jsonReader.Read())
				return null;

			var jsonSerializer = new JsonSerializer();
			jsonSerializer.Converters.Add(new ExpandoObjectConverter());

			object jsonObject;
			if (jsonReader.TokenType == JsonToken.StartArray)
				jsonObject = jsonSerializer.Deserialize<List<ExpandoObject>>(jsonReader);
			else
				jsonObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);

			var backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			AddToBackingStore(backingStore, string.Empty, jsonObject);
			return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
		}

		private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
		{
			var d = value as IDictionary<string, object>;
			if (d != null)
			{
				foreach (var entry in d)
				{
					AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
				}
				return;
			}

			var l = value as IList;
			if (l != null)
			{
				for (var i = 0; i < l.Count; i++)
				{
					AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
				}
				return;
			}

			backingStore[prefix] = value;
		}

		private static string MakeArrayKey(string prefix, int index)
		{
			return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
		}

		private static string MakePropertyKey(string prefix, string propertyName)
		{
			return (string.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
		}

		#endregion
	}
}