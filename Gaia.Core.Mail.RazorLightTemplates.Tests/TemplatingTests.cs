using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Gaia.Core.Mail.Configuration;
using Xunit;

namespace Gaia.Core.Mail.RazorLightTemplates.Tests
{
	public class TemplatingTests
	{
		private readonly string _templatePath;
		public TemplatingTests()
		{
			_templatePath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName,
				"TestTemplates");
		}

		[Fact]
		public async Task TestSimpleMsg()
		{
			var provider = new RazorLightMessageProvider(new EmailTemplateConfiguration(_templatePath));
			var res = await provider.GetCompiledMessageAsync("test", "@Model.MessagePlain", new {
				MessagePlain = "plain message bla bla bla"
			});

			Assert.False(string.IsNullOrEmpty(res));
			Assert.True(res.Trim() == "plain message bla bla bla");
		}
	}
}
