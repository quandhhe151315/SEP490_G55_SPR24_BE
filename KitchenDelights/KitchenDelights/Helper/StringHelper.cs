﻿using System.Text;
using System.Text.RegularExpressions;

namespace KitchenDelights.Helper
{
    public class StringHelper
    {
        public static string GenerateRandomString(int length)
        {
            StringBuilder builder = new();
            Random random = new();

            while (builder.Length < length)
            {
                //Random between 48 and 123 to cover all alphanumeric letters
                char character = Convert.ToChar(random.Next(48, 123));
                if (Char.IsLetterOrDigit(character))
                {
                    builder.Append(character);
                }
            }

            return builder.ToString();
        }

        public static string Process(string originalString)
        {
            string newString = Regex.Replace(originalString, @"\s+", " ")
                               .ToLower()
                               .Trim();
            return newString;
        }
    }
}
