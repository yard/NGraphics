using System.Collections.Generic;

namespace NGraphics.Custom.Models.Brushes
{
    public abstract class GradientBrush : BaseBrush
    {
        public readonly List<GradientStop> Stops = new List<GradientStop>();

        public void AddStop(double offset, Color color)
        {
            Stops.Add(new GradientStop(offset, color));
        }
    }
}