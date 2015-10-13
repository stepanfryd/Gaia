using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
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

		public byte[] GeneratePdfOutput(ControllerContext context, object model = null, string viewName = null,
			Action<PdfWriter, Document> configureSettings = null)
		{
			if (viewName == null)
			{
				viewName = context.RouteData.GetRequiredString("action");
			}

			context.Controller.ViewData.Model = model;
			var html = RenderRazorView(context, viewName);

			byte[] output;
			using (var capturedActionStream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
			{
				var memoryStream = new MemoryStream();
				var document = new Document(PageSize.A4, 30, 30, 10, 10);
				//to create landscape, use PageSize.A4.Rotate() for pageSize
				var writer = PdfWriter.GetInstance(document, memoryStream);
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

		public string RenderRazorView(ControllerContext context, string viewName)
		{
			var viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
			var sb = new StringBuilder();

			using (TextWriter tr = new StringWriter(sb))
			{
				var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
					context.Controller.TempData, tr);
				viewEngineResult.Render(viewContext, tr);
			}
			return sb.ToString();
		}

		#endregion
	}
}