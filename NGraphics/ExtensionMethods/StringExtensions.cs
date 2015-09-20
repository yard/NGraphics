using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NGraphics.Custom.ExtensionMethods
{
    public static class StringExtensions
    {
        public static List<double> ToPointValues(this string stringValue)
        {
            return Regex.Split(stringValue.Remove(0, 1), @"[\s,]|(?=-)")
                .Where(c => !string.IsNullOrEmpty(c))
                .Select(c => double.Parse(c))
                .ToList();
        }
    }
}