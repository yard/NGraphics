using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    [TestFixture]
   public class TextTests : SvgReaderTestBase
    {
        #region Mozilla Text

        [Test]
        public void MozillaText1()
        {
            ReadAndDraw("W3C.Text.Text1.svg");
        }

        [Test]
        public void MozillaText2()
        {
            ReadAndDraw("W3C.Text.Text2.svg");
        }

        [Test]
        public void MozillaText3()
        {
            ReadAndDraw("W3C.Text.Text3.svg");
        }

        [Test]
        public void MozillaText4()
        {
            ReadAndDraw("W3C.Text.Text4.svg");
        }

        #endregion Mozilla Text
    }
}
