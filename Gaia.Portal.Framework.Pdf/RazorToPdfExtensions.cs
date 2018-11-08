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

/*
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
			Rectangle pageSize = null, Action<PdfWriter, Document> configureSettings = null)
		{
			return new RazorToPdf().GeneratePdfOutput(context, model, viewName, pageSize, configureSettings);
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
*/