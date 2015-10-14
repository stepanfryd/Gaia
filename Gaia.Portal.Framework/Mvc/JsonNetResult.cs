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
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Gaia.Portal.Framework.Mvc
{
	public class JsonNetResult : ActionResult
	{
		#region Public members

		public Encoding ContentEncoding { get; set; }
		public string ContentType { get; set; }
		public object Data { get; set; }

		public JsonSerializerSettings SerializerSettings { get; set; }
		public Formatting Formatting { get; set; }

		#endregion

		#region Constructors

		public JsonNetResult()
		{
			SerializerSettings = new JsonSerializerSettings();

			//SerializerSettings.Converters.Add(new JavaScriptDateTimeConverter());
			//SerializerSettings.Converters.Add(new IsoDateTimeConverter());
			Formatting = Formatting.None;
		}

		public JsonNetResult(object data)
			: this()
		{
			Data = data;
		}

		#endregion

		#region Private and protected

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			var response = context.HttpContext.Response;
			response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null) return;
			var writer = new JsonTextWriter(response.Output) {Formatting = Formatting};

			var serializer = JsonSerializer.Create(SerializerSettings);
			serializer.Serialize(writer, Data);
			writer.Flush();
		}

		#endregion
	}


	/*
	public class JsonNetResult : JsonResult
	{
		public JsonNetResult()
		{
			Settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Error
			};
			Settings.Converters.Add(new JavaScriptDateTimeConverter());
		}

		public JsonSerializerSettings Settings { get; private set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
			    string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("JSON GET is not allowed");

			HttpResponseBase response = context.HttpContext.Response;
			response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;
			if (Data == null)
				return;

			JsonSerializer scriptSerializer = JsonSerializer.Create(Settings);
			using (var sw = new StringWriter())
			{
				scriptSerializer.Serialize(sw, Data);
				response.Write(sw.ToString());
			}

			//response.Write(JsonConvert.SerializeObject(Data, new JavaScriptDateTimeConverter()));
		}
	}

	*/
}