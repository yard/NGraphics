using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGraphics.Models
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(Point moveTo)
        {
            this.Start = moveTo;
            this.End = moveTo;
        }

        public override void AddToPath(Path graphicsPath)
        {
			graphicsPath.MoveTo(Start,End,false);
        }

        public override string ToString()
        {
            return "M" + this.Start.ToSvgString();
        }

    }
}
