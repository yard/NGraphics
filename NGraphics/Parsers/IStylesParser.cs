using System.Collections.Generic;
using System.Xml.Linq;
using NGraphics.Custom.Models;
using NGraphics.Custom.Models.Brushes;

namespace NGraphics.Custom.Parsers
{
  public interface IStylesParser
  {
    Pen GetPen(Dictionary<string, string> styleAttributes);
    BaseBrush GetBrush(Dictionary<string, string> styleAttributes,Dictionary<string, XElement> defs, Pen pen);
      Dictionary<string, string> ParseStyleValues(string styleString);
  }
}