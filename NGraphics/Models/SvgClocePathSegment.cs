using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGraphics.Models
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(Path graphicsPath)
        {
            //todo May need to fix
            //// Important for custom line caps.  Force the path the close with an explicit line, not just an implicit close of the figure.
            //if (graphicsPath.PointCount > 0 && !graphicsPath.PathPoints[0].Equals(graphicsPath.PathPoints[graphicsPath.PathPoints.Length - 1]))
            //{
            //    int i = graphicsPath.PathTypes.Length - 1;
            //    while (i >= 0 && graphicsPath.PathTypes[i] > 0) i--;
            //    if (i < 0) i = 0;
            //    graphicsPath.AddLine(graphicsPath.PathPoints[graphicsPath.PathPoints.Length - 1], graphicsPath.PathPoints[i]);
            //}
            graphicsPath.Close();
        }

        public override string ToString()
        {
            return "z";
        }

    }
}
