using System;

namespace OmmaLicenseValidator.Common
{
    public static class ExtensionMethods
    {
        public static string CleanString(this string str) => 
            string.IsNullOrWhiteSpace(str) ? 
                str : str.Replace("\r\n", string.Empty).Trim();

        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return default(DateTime);

            return !DateTime.TryParse(str, out var parsedDate) ? 
                default(DateTime) : parsedDate;
        }
    }
}
