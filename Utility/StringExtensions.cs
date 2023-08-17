namespace Utility
{
    public static class StringExtensions
    {
        public static string? GetDefaultIfNullOrWhiteSpace(this string value, string? defaultValue = null) 
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return value;
        }
    }
}