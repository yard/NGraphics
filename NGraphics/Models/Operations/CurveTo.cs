namespace NGraphics.Models.Operations
{
    public class CurveTo : PathOperation
    {
        public CurveTo(Point start, Point firstControlPoint, Point secondControlPoint)
        {
            Start = start;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
        }

        public CurveTo(Point start, Point firstControlPoint, Point secondControlPoint, Point end)
        {
            Start = start;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
            End = end;
        }

        public Point End;
        public Point FirstControlPoint;
        public Point SecondControlPoint;
        public Point Start;

    }
}