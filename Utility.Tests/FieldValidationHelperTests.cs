
namespace Utility.Tests
{
    public class FieldValidationHelperTests
    {
        [Theory]
        [InlineData(null, true, null, null, true)]
        [InlineData(null, true, 2, 10, true)]
        [InlineData(null, false, null, null, false)]
        [InlineData(null, false, 2, 10, false)]
        [InlineData(5, false, 3, null, true)]
        [InlineData(5, false, 10, null, false)]
        [InlineData(5, false, null, 10, true)]
        [InlineData(5, false, null, 3, false)]
        [InlineData(5, false, 3, 10, true)]
        [InlineData(5, false, 3, 4, false)]
        [InlineData(5, false, 8, 10, false)]
        [InlineData(5, false, 5, 10, true)]
        [InlineData(5, false, 3, 5, true)]
        public void Should_Return_Correctly_For_ValidateNumber(int? number, bool allowNull, int? minValue, int? maxValue, bool expectedResult)
        {
            var validationResult = number.ValidateNumber(allowNull, minValue, maxValue);

            Assert.Equal(expectedResult, validationResult);
        }


        [Theory]
        [InlineData(null, true, null, null, true)]
        [InlineData(null, true, 2, 10, true)]
        [InlineData(null, false, null, null, false)]
        [InlineData(null, false, 2, 10, false)]
        [InlineData("apple", true, null, null, true)]
        [InlineData("apple", false, 3, null, true)]
        [InlineData("apple", false, 10, null, false)]
        [InlineData("apple", false, null, 10, true)]
        [InlineData("apple", false, null, 3, false)]
        [InlineData("apple", false, 3, 10, true)]
        [InlineData("apple", false, 3, 4, false)]
        [InlineData("apple", false, 8, 10, false)]
        [InlineData("apple", false, 3, 5, true)]
        [InlineData("apple", false, 5, 10, true)]
        public void Should_Return_Correctly_For_ValidateString(string? text, bool allowNullOrWhiteSpace, int? minLength, int? maxLength, bool expectedResult)
        {
            var validationResult = text.ValidateString(allowNullOrWhiteSpace, minLength, maxLength);

            Assert.Equal(expectedResult, validationResult);
        }

        [Theory]
        [InlineData(null, true, true)]
        [InlineData(null, false, false)]
        [InlineData("test@test", true, true)]
        [InlineData("test@test", false, true)]
        [InlineData("test@test.com", false, true)]
        [InlineData("test@", false, false)]
        [InlineData("@test", false, false)]
        [InlineData("@test@test", false, false)]
        public void Should_Return_Correctly_For_ValidateAsEmail(string? text, bool allowNullOrWhiteSpace, bool expectedResult)
        {
            var validationResult = text.ValidateAsEmail(allowNullOrWhiteSpace);

            Assert.Equal(expectedResult, validationResult);
        }

        [Theory]
        [InlineData(new bool[] { true, true, true }, false, true)]
        [InlineData(new bool[] { false, false, false }, false, true)]
        [InlineData(new bool[] { }, false, true)]
        [InlineData(new bool[] { true, true, true }, true, false)]
        [InlineData(new bool[] { true, true, false }, true, false)]
        [InlineData(new bool[] { false, false, false }, true, false)]
        [InlineData(new bool[] { }, true, false)]
        [InlineData(new bool[] { true, false, false}, true, true)]
        public void Should_Return_Correctly_For_ValidateBoolGroup(bool[] bools, bool shouldContainOneTrue, bool expectedResult)
        {
            var validationResult = bools.ValidateBoolGroup(shouldContainOneTrue);

            Assert.Equal(expectedResult, validationResult);
        }
    }
}