using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGraphics.Interfaces;

namespace NGraphics
{
    public class Graphic : IDrawable
    {
        public Graphic(Size size, Rect viewBox)
        {
            Size = size;
            ViewBox = viewBox;
        }

        public Graphic(Size size)
            : this(size, new Rect(Point.Zero, size))
        {
        }

        public readonly List<IDrawable> Children = new List<IDrawable>();
        public string Description = "";
        public Size Size;
        public string Title = "";
        public Rect ViewBox;

        public void Draw(ICanvas canvas)
        {
            canvas.SaveState();

            //
            // Scale the viewBox into the size
            //
            var sx = 1.0;
            if (ViewBox.Width > 0)
            {
                sx = Size.Width/ViewBox.Width;
            }
            var sy = 1.0;
            if (ViewBox.Height > 0)
            {
                sy = Size.Height/ViewBox.Height;
            }

            canvas.Scale(sx, sy);
            canvas.Translate(-ViewBox.X, -ViewBox.Y);

            //
            // Draw
            //
            foreach (var c in Children)
            {
                c.Draw(canvas);
            }

            canvas.RestoreState();
        }

        public static Graphic LoadSvg(TextReader reader)
        {
            var svgr = new SvgReader(reader);
            return svgr.Graphic;
        }

        public override string ToString()
        {
            try
            {
                if (Children.Count == 0)
                    return "Graphic";
                var w =
                    Children.
                        GroupBy(x => x.GetType().Name).
                        Select(x => x.Count() + " " + x.Key);
                return "Graphic with " + string.Join(", ", w);
            }
            catch
            {
                return "Graphic with errors!";
            }
        }
    }
}