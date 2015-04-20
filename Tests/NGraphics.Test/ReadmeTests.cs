using NUnit.Framework;
using System.IO;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Models;
using NGraphics.Models.Operations;
using Path = NGraphics.Models.Elements.Path;

namespace NGraphics.Test
{
	[TestFixture]
	public class ReadmeTests : PlatformTest
	{
		// http://app.coolors.co/dcdcdd-c5c3c6-46494c-4c5c68-4183c4
		// http://app.coolors.co/dcdcdd-c5c3c6-46494c-4c5c68-68a5e2
		[Test]
		public async Task Icon ()
		{
			var size = new Size (64);
			var canvas = Platforms.Current.CreateImageCanvas (size, scale: 2);
			canvas.SaveState ();
			canvas.Scale (size);
			canvas.Translate (1 / 8.0, 0);

			var p = new Path ();
			p.MoveTo (0, 1,false);
			p.LineTo (0, 0, false);
			p.LineTo (0.5, 1, false);
			p.LineTo (0.5, 0,false);

			var colors = new [] {
				"#DCDCDD",
				"#C5C3C6",
				"#46494C",
				"#4C5C68",
				"#68A5E2",
			};
			foreach (var c in colors) {
				p.Pen = new Pen (c, 1 / 4.0);
				p.Draw (canvas);
				canvas.Translate (1 / 16.0, 0);
			}
      await SaveImage (canvas, "Icon.png");
		}

		[Test]
    public async Task Example1()
		{
			var canvas = Platforms.Current.CreateImageCanvas (new Size (100), scale: 2);

			canvas.DrawEllipse (10, 20, 30, 30, Pens.Red, Brushes.White);
			canvas.DrawRectangle (40, 50, 60, 70, brush: Brushes.Blue);
			canvas.DrawPath (new PathOperation[] {	
				new MoveTo (100, 100, false),
				new LineTo (50, 100,false),
				new LineTo (50, 0, false),
				new ClosePath ()
			}, brush: Brushes.Gray);

      await SaveImage(canvas, "Example1.png");
		}

		[Test]
		public async Task PenWidths ()
		{
			var canvas = Platforms.Current.CreateImageCanvas (new Size (120*5, 120), scale: 2);

			canvas.Translate (20, 20);
			for (var i = 0; i < 5; i++) {
				canvas.DrawEllipse (
					new Rect (new Size (80)),
					pen: Pens.DarkGray.WithWidth (1 << i),
					brush: Brushes.LightGray);
				canvas.Translate (120, 0);
			}

      await SaveImage(canvas, "PenWidths.png");
		}
	}

}

