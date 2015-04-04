using System.Xml.Linq;

namespace NGraphics.Parsers
{
  public interface IValuesParser
  {
    double ReadNumber(XAttribute a);
    double ReadNumber(string raw);
  }
}