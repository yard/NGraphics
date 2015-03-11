using System;
using NGraphics.Codes;

namespace NGraphics.Parsers
{
  public class OperationParser
  {
    public static OperationType Parse(char operation)
    {
      switch (char.ToUpper(operation))
      {
        case 'M':
          return OperationType.MoveTo;
        case 'L':
          return OperationType.LineTo;
        case 'C':
          return OperationType.CurveTo;
        case 'S':
          return OperationType.ContinueCurveTo;
        case 'A':
          return OperationType.ArcTo;
        case 'Z':
          return OperationType.Close;
        default:
          throw new NotSupportedException(string.Format("Operation {0} not supported", operation));
      }
    }
  }
}