using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    [TestFixture]
   public class TextTests : SvgReaderTestBase
    {
        [Test]
        public void Text1()
        {
            ReadAndDraw("W3C.Text.Text1.svg");
        }

        [Test]
        public void Text2()
        {
            ReadAndDraw("W3C.Text.Text2.svg");
        }

        [Test]
        public void Text3()
        {
            ReadAndDraw("W3C.Text.Text3.svg");
        }

        [Test]
        public void Text4()
        {
            ReadAndDraw("W3C.Text.Text4.svg");
        }

        [Test]
        public void TextAlign01b()
        {
            ReadAndDraw("W3C.Text.text-align-01-b.svg");
        }
    }
}
