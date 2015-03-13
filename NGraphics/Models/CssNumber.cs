using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
