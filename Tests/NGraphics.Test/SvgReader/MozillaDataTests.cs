using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    [TestFixture]
    public class MozillaDataTests : SvgReaderTestBase
    {
        [Test]
        public void MozillaPath_Data_01()
        {
            ReadAndDraw("mozilla.paths-data-01-t.svg");
        }

        [Test]
        public void MozillaPath_Data_02()
        {
            ReadAndDraw("mozilla.paths-data-02-t.svg");
        }
        [Test]
        public void MozillaPath_Data_04()
        {
            ReadAndDraw("mozilla.paths-data-04-t.svg");
        }

        [Test]
        public void MozillaPath_Data_05()
        {
            ReadAndDraw("mozilla.paths-data-05-t.svg");
        }

        [Test]
        public void MozillaPath_Data_06()
        {
            ReadAndDraw("mozilla.paths-data-06-t.svg");
        }

        [Test]
        public void MozillaPath_Data_07()
        {
            ReadAndDraw("mozilla.paths-data-07-t.svg");
        }

        [Test]
        public void MozillaPath_Data_08()
        {
            ReadAndDraw("mozilla.paths-data-08-t.svg");
        }

        [Test]
        public void MozillaPath_Data_09()
        {
            ReadAndDraw("mozilla.paths-data-09-t.svg");
        }

        [Test]
        public void MozillaPath_Data_10()
        {
            ReadAndDraw("mozilla.paths-data-10-t.svg");
        }

        [Test]
        public void MozillaPath_Data_12()
        {
            ReadAndDraw("mozilla.paths-data-12-t.svg");
        }

        [Test]
        public void MozillaPath_Data_13()
        {
            ReadAndDraw("mozilla.paths-data-13-t.svg");
        }

        [Test]
        public void MozillaPath_Data_14()
        {
            ReadAndDraw("mozilla.paths-data-14-t.svg");
        }


        [Test]
        public void MozillaPath_Data_15()
        {
            ReadAndDraw("mozilla.paths-data-15-t.svg");
        }

        [Test]
        public void MozillaPath_Data_16()
        {
          ReadAndDraw("mozilla.paths-data-16-t.svg");
        }

        [Test]
        public void MozillaTransform()
        {
            ReadAndDraw("mozilla.transform.svg");
        }

        [Test]
        public void MozillaBezierCurves1()
        {
            ReadAndDraw("mozilla.BezierCurves1.svg");
        }

        [Test]
        public void MozillaBezierCurves2()
        {
            ReadAndDraw("mozilla.BezierCurves2.svg");
        }

        [Test]
        public void MozillaEllipse()
        {
            ReadAndDraw("mozilla.ellipse.svg");
        }

        [Test]
        public void MozillaPath()
        {
            ReadAndDraw("mozilla.path.svg");
        }
    }
}
