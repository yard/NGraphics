using System;
using System.Collections.Generic;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Operations;

namespace NGraphics
{
    public class GraphicCanvas : ICanvas
    {
        public GraphicCanvas(Size size)
        {
            Graphic = new Graphic(size);
        }

        public Graphic Graphic { get; private set; }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public void Transform(Transform transform)
        {
            throw new NotImplementedException();
        }

        public void RestoreState()
        {
            throw new NotImplementedException();
        }

        public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left,
            Pen pen = null, BaseBrush baseBrush = null)
        {
            throw new NotImplementedException();
        }

        public void DrawPath(IEnumerable<PathOperation> commands, Pen pen = null, BaseBrush baseBrush = null)
        {
            Graphic.Children.Add(new Path(commands, pen, baseBrush));
        }

        public void DrawRectangle(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
        {
            Graphic.Children.Add(new Rectangle(frame, pen, baseBrush));
        }

        public void DrawEllipse(Rect frame, Pen pen = null, BaseBrush baseBrush = null)
        {
            Graphic.Children.Add(new Ellipse(frame, pen, baseBrush));
        }

        public void DrawImage(IImage image, Rect frame)
        {
            throw new NotImplementedException();
        }
    }
}