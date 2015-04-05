using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    [TestFixture]
    public class ComplexPathsTests : SvgReaderTestBase
    {
        [Test]
        public void Smile()
        {
            ReadAndDraw("Smile.svg");
        }

        [Test]
        public void iPod()
        {
            ReadAndDraw("IPod.svg");
        }

        [Test]
        public void SunAtNight()
        {
            ReadAndDraw("SunAtNight.svg");
        }

        [Test]
        public void MocastIcon()
        {
            ReadAndDraw("MocastIcon.svg");
        }

        [Test]
        public void Ghost()
        {
            ReadAndDraw("horseshoe.svg");
        }

        [Test]
        public void ErulisseuiinSpaceshipPack()
        {
            ReadAndDraw("ErulisseuiinSpaceshipPack.svg");
        }
    }
}
