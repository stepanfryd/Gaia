namespace Gaia.Core.Tests.TestObjects
{
	public interface ITestInterface
	{
		string Value { get; set; }

		string GetValue(int value);
	}
}