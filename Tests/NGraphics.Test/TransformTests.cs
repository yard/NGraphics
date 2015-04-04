using NUnit.Framework;
using System.IO;
using System;
using System.Reflection;
using NGraphics.Codes;
using NGraphics.Interfaces;
using NGraphics.Models;

namespace NGraphics.Test
{
	[TestFixture]
	public class TransformTests : PlatformTest
	{
		[Test]
		public void RotateTranslate ()
		{
			var canvas = Platforms.Current.CreateImageCanvas (new Size (200));

			canvas.Rotate (30);
			canvas.Translate (50, 50);

			canvas.DrawRectangle (0, 0, 150, 75, baseBrush: Brushes.Red);

			canvas.GetImage ().SaveAsPng (GetPath ("TransformRotateTranslate.png"));
		}

		[Test]
		public void TranslateRotate ()
		{
			var canvas = Platforms.Current.CreateImageCanvas (new Size (200));

			canvas.Translate (50, 50);
			canvas.Rotate (30);

			canvas.DrawRectangle (0, 0, 150, 75, baseBrush: Brushes.Red);

			canvas.GetImage ().SaveAsPng (GetPath ("TransformTranslateRotate.png"));
		}
	}
}

