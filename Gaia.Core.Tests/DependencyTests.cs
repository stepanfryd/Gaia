using Gaia.Core.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gaia.Core.Tests
{
	[TestClass]
	public class DependencyTests : TestBase
	{
		[TestMethod]
		public void InterfaceResolveTest()
		{
			var monitorService = Container.Resolve<ITestInterface>();
			var testValInt = 123;
			var testValStr = "DEFAULT_NEW";
			Assert.IsNotNull(monitorService);
			Assert.IsTrue(monitorService.Value == "DEFAULT");
			Assert.IsTrue(monitorService.GetValue(testValInt) == $"DEFAULT-{testValInt}");
			monitorService.Value = testValStr;
			Assert.IsTrue(monitorService.GetValue(testValInt) == $"{testValStr}-{testValInt}");
		}
	}
}