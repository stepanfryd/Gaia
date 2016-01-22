using System.Runtime.Remoting;
using Gaia.Core.Tests.TestObjects;
using NUnit.Framework;

namespace Gaia.Core.Tests
{
	[TestFixture]
	public class DependencyTests : TestBase
	{
		[Test]
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