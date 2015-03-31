using System.Globalization;

namespace NGraphics.Models
{
    public sealed class CssNumber
    {
      private static NumberFormatInfo format;

      public static NumberFormatInfo Format
      {
        get
        {
          if (format == null)
          {
            format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
          }

          return format;
        }
      }
    }

}
