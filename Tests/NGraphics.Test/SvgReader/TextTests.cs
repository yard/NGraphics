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
            ReadAndDraw("mozilla.Text1.svg");
        }

        [Test]
        public void MozillaText2()
        {
            ReadAndDraw("mozilla.Text2.svg");
        }

        [Test]
        public void MozillaText3()
        {
            ReadAndDraw("mozilla.Text3.svg");
        }

        [Test]
        public void MozillaText4()
        {
            ReadAndDraw("mozilla.Text4.svg");
        }

        #endregion Mozilla Text
    }
}
