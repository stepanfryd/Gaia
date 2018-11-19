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
using Gaia.Core.Mail.Configuration;
using RazorLight;
using System.Threading.Tasks;

namespace Gaia.Core.Mail.RazorLightTemplates
{
	public class RazorLightMessageProvider : IMessageProvider
	{
		private RazorLightEngine _razorEngine;

		public RazorLightMessageProvider(IEmailTemplateConfiguration templateConfiguration)
		{
			_razorEngine = new RazorLightEngineBuilder()
				.UseFilesystemProject(templateConfiguration.TemplateFolder)
				.UseMemoryCachingProvider()
				.Build();
		}

		public async Task<string> GetCompiledMessageAsync<T>(string templateKey, string messageTemplate, T model)
		{
			string retVal;

			var cacheResult = _razorEngine.TemplateCache.RetrieveTemplate(templateKey);
			if (cacheResult.Success)
			{
				retVal = await _razorEngine.RenderTemplateAsync(cacheResult.Template.TemplatePageFactory(), model);
			}
			else
			{
				retVal = await _razorEngine.CompileRenderAsync(templateKey, messageTemplate, model);
			}

			return retVal;
		}
	}
}