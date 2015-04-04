using NUnit.Framework;
using System.IO;
using System;
using System.Reflection;
using NGraphics.Codes;
using NGraphics.Models;
using NGraphics.Models.Brushes;

namespace NGraphics.Test
{
	[TestFixture]
	public class BrushTests : PlatformTest
	{
		[Test]
		public void RectLinearGradient ()
		{
			var canvas = Platforms.Current.CreateImageCanvas (new Size (100));

			var rect = new Rect (0, 10, 100, 80);
			var brush = new LinearGradientBrush (
				Point.Zero,
				Point.OneY,
				Colors.Green,
				Colors.LightGray);				

			canvas.DrawRectangle (rect, baseBrush: brush);

			canvas.GetImage ().SaveAsPng (GetPath ("Brush.RectLinearGradient.png"));
		}
	}
}

