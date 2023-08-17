
namespace Utility.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "testNull", "testNull")]
        [InlineData("", "testEmpty", "testEmpty")]
        [InlineData("   ", "testWhiteSpace", "testWhiteSpace")]
        [InlineData("Exists", "testExists", "Exists")]
        public void Should_Return_Correctly_For_GetDefaultIfNullOrWhiteSpace(string? text, string defaultValue, string? expectedResult)
        {
            var validationResult = text.GetDefaultIfNullOrWhiteSpace(defaultValue);

            Assert.Equal(expectedResult, validationResult);
        }
    }
}