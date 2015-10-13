using System;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

		public override void ExecuteResult(ControllerContext context)
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

				new FileContentResult(context.GeneratePdf(Model, ViewName, ConfigureSettings), "application/pdf")
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