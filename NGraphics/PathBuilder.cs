using System.Collections.Generic;
using System.Linq;
using NGraphics.Codes;
using NGraphics.ExtensionMethods;
using NGraphics.Parsers;

namespace NGraphics
{
    public static class PathBuilder
    {
        public static void ToAbsolute(this Point point, List<string> segments)
        {
            //var point = new Point(x, y);

            //if ((isRelativeX || isRelativeY) && segments.Count > 0)
            //{
            var lastSegment = segments.Last();

            // if the last element is a SvgClosePathSegment the position of the previous element should be used because the position of SvgClosePathSegment is 0,0
            var operation = OperationParser.Parse(lastSegment);
            if (operation.Type == OperationType.Close)
            {
                //lastSegment = segments.Reverse().OfType<SvgMoveToSegment>().First();
            }

            //if (isRelativeX)
            //{
            point.X += lastSegment.ToPointValues()[0];
            //}

            //if (isRelativeY)
            //{
            point.Y += lastSegment.ToPointValues()[1];
            //}

        }
    }
}
