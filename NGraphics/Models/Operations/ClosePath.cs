using System;

namespace NGraphics
{
    public class ClosePath : PathOperation
    {
        public override Point GetContinueCurveControlPoint()
        {
            throw new NotSupportedException();
        }
    }
}