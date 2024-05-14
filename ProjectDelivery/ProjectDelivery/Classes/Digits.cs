using System.Text.RegularExpressions;

namespace ProjectDelivery.Classes
{
    public class Digits
    {
        public static string ExtractDigits(string input)
        {
            return Regex.Replace(input, "[^0-9]", "");
        }
    }
}
