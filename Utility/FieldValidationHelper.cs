using System.Net.Mail;

namespace Utility
{
    public static class FieldValidationHelper
    {
        public static bool ValidateNumber(this int? number, bool allowNull = false, int? minValue = null, int? maxValue = null)
        {
            if (!allowNull && !number.HasValue)
            {
                return false;
            }

            if (number.HasValue && minValue.HasValue && number < minValue)
            {
                return false;
            }

            if (number.HasValue && maxValue.HasValue && number > maxValue)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateString(this string target, bool allowNullOrWhiteSpace = false, int? minLength = null, int? maxLength = null)
        {
            if (!allowNullOrWhiteSpace && string.IsNullOrWhiteSpace(target))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(target) && minLength.HasValue && target.Length < minLength)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(target) && maxLength.HasValue && target.Length > maxLength)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateAsEmail(this string target, bool allowNullOrWhiteSpace = false)
        {
            if (!allowNullOrWhiteSpace && string.IsNullOrWhiteSpace(target))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(target))
            {
                try
                {
                    MailAddress m = new MailAddress(target);

                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateBoolGroup(this bool[] targets, bool shouldContainOneTrue = true)
        {
            var validationException = new ArgumentException($"{string.Join(",", targets)} is invalid", nameof(targets));

            if (shouldContainOneTrue && targets.Where(t => t == true).Count() != 1)
            {
                return false;
            }

            return true;
        }
    }
}
