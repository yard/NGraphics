using System.Collections.Generic;
using System.Xml.Linq;
using NGraphics.Models;
using NGraphics.Models.Brushes;

namespace NGraphics.Parsers
{
  public interface IStylesParser
  {
    Pen GetPen(Dictionary<string, string> styleAttributes);
    BaseBrush GetBrush(Dictionary<string, string> styleAttributes,Dictionary<string, XElement> defs, Pen pen);
  }
}