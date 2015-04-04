using System.IO;
using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    public class SvgReaderTestBase : PlatformTest
    {
        Graphic Read(string path)
        {
            using (var s = OpenResource(path))
            {
              var r = new NGraphics.Parsers.SvgReader(new StreamReader(s));
                Assert.GreaterOrEqual(r.Graphic.Children.Count, 0);
                Assert.Greater(r.Graphic.Size.Width, 1);
                Assert.Greater(r.Graphic.Size.Height, 1);
                return r.Graphic;
            }
        }

        protected void ReadAndDraw(string path)
        {
            var g = Read(path);
            var c = Platform.CreateImageCanvas(g.Size);
            g.Draw(c);
            c.GetImage().SaveAsPng(GetPath(path));
        }
    }
}
