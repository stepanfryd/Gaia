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
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;

namespace Gaia.Portal.Framework.Pdf
{
	public class PdfActionResult : ActionResult
	{
		#region Public members

		public string ViewName { get; set; }
		public object Model { get; set; }
		public Action<PdfWriter, Document> ConfigureSettings { get; set; }

		public string FileDownloadName { get; set; }

		#endregion

		#region Constructors

		public PdfActionResult(string viewName, object model) : this(model, viewName)
		{
		}

		public PdfActionResult(object model, Action<PdfWriter, Document> configureSettings = null)
			: this(model, null, configureSettings)
		{
		}

		public PdfActionResult(object model, string viewName = null, Action<PdfWriter, Document> configureSettings = null)
		{
			ViewName = viewName;
			Model = model;

			if (configureSettings != null)
				ConfigureSettings = configureSettings;
		}

		#endregion

		#region Private and protected

		public override void ExecuteResult(ActionContext context)
		{
			//IView viewEngineResult;
			//ViewContext viewContext;

			if (ViewName == null)
			{
				ViewName = context.RouteData.GetRequiredString("action");
			}

			context.Controller.ViewData.Model = Model;


			if (context.HttpContext.Request.QueryString["html"] != null &&
			    context.HttpContext.Request.QueryString["html"].ToLower().Equals("true"))
			{
				RenderHtmlOutput(context);
			}
			else
			{
				if (!string.IsNullOrEmpty(FileDownloadName))
				{
					context.HttpContext.Response.AddHeader("content-disposition",
						"attachment; filename=" + FileDownloadName);
				}

				new FileContentResult(context.GeneratePdf(Model, ViewName, PageSize.A4, ConfigureSettings), "application/pdf")
					.ExecuteResult(context);
			}
		}

		private void RenderHtmlOutput(ControllerContext context)
		{
			var viewEngineResult = ViewEngines.Engines.FindView(context, ViewName, null).View;
			var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
				context.Controller.TempData, context.HttpContext.Response.Output);
			viewEngineResult.Render(viewContext, context.HttpContext.Response.Output);
		}

		#endregion
	}
}
*/