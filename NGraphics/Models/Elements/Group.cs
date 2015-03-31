using System.Collections.Generic;
using NGraphics.Interfaces;

namespace NGraphics.Models.Elements
{
    public class Group : Element
    {
        public Group()
            : base(null, null)
        {
        }

        public readonly List<IDrawable> Children = new List<IDrawable>();

        protected override void DrawElement(ICanvas canvas)
        {
            foreach (var c in Children)
            {
                c.Draw(canvas);
            }
        }
    }
}