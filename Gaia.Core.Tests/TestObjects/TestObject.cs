using System;

namespace Gaia.Core.Tests.TestObjects
{
	public class TestObject : ITestInterface
	{
		public TestObject()
		{
			Value = "DEFAULT";
		}

		public string Value { get; set; }

		public string GetValue(int value)
		{
			return $"{Value}-{value}";
		}
	}
}