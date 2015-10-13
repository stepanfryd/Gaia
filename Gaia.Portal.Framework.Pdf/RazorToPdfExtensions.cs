using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Gaia.Portal.Framework.Pdf
{
	public static class RazorToPdfExtensions
	{
		#region Private and protected

		public static byte[] GeneratePdf(this ControllerContext context, object model = null, string viewName = null,
			Action<PdfWriter, Document> configureSettings = null)
		{
			return new RazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
		}

		public static PdfActionResult Pdf(this Controller controller, string viewName, object model,
			string fileDownloadName = null)
		{
			return Pdf(controller, model, viewName, fileDownloadName);
		}

		public static PdfActionResult Pdf(this Controller controller, string viewName, string fileDownloadName = null)
		{
			return Pdf(controller, null, viewName, fileDownloadName);
		}

		public static PdfActionResult Pdf(this Controller controller, object model, string fileDownloadName = null)
		{
			return Pdf(controller, model, null, fileDownloadName);
		}

		public static PdfActionResult Pdf(this Controller controller, object model, string viewName, string fileDownloadName,
			Action<PdfWriter, Document> configureSettings = null)
		{
			var retVal = new PdfActionResult(model, viewName, configureSettings);

			if (!string.IsNullOrEmpty(fileDownloadName)) retVal.FileDownloadName = fileDownloadName;

			return retVal;
		}

		#endregion
	}
}