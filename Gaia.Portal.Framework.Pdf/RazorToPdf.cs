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
using System.Text;
using System.Web.Mvc;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace Gaia.Portal.Framework.Pdf
{
	public class RazorToPdf
	{
		#region Fields and constants

		private const string DEFAULT_FONT_NAME = "arial.ttf";

		#endregion

		#region Private and protected

		public byte[] GeneratePdfOutput(ControllerContext controllerContext, object model = null, string viewName = null,
			Rectangle pageSize = null, Action<PdfWriter, Document> configureSettings = null)
		{
			controllerContext.Controller.ViewData.Model = model;

			if (viewName == null)
			{
				viewName = controllerContext.RouteData.GetRequiredString("action");
			}
			var html = RenderView(controllerContext, viewName);

			// fix potentionally mallformed XHTML document
			var htmlDoc = new HtmlDocument()
			{
				OptionCheckSyntax = true,
				OptionWriteEmptyNodes = true,
				OptionAutoCloseOnEnd = true,
				OptionFixNestedTags= true,
				OptionOutputAsXml = true
			};
			htmlDoc.LoadHtml(html);

			var htmlFixed = htmlDoc.DocumentNode.WriteTo();

			byte[] output;
			using (var capturedActionStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlFixed)))
			{
				var memoryStream = new MemoryStream();
				var document = new Document(pageSize ?? PageSize.A4, 30, 30, 10, 10);
				//to create landscape, use PageSize.A4.Rotate() for pageSize
				var writer = PdfWriter.GetInstance(document, memoryStream);

				configureSettings?.Invoke(writer, document);

				var worker = XMLWorkerHelper.GetInstance();
				var defaultFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), DEFAULT_FONT_NAME);
				var fontProvider = new CustomFontFactory(defaultFontPath);

				document.Open();
				worker.ParseXHtml(writer, document, capturedActionStream, null, fontProvider);
				writer.CloseStream = false;
				document.Close();
				memoryStream.Position = 0;

				output = memoryStream.GetBuffer();
			}
			return output;
		}

		public string RenderView(ControllerContext context, string viewName)
		{
			var engineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
			var sb = new StringBuilder();

			using (var writer = new StringWriter(sb))
			{
				var viewContext = new ViewContext(context, engineResult, context.Controller.ViewData,
					context.Controller.TempData, writer);
				engineResult.Render(viewContext, writer);
			}
			return sb.ToString();
		}

		#endregion
	}
}