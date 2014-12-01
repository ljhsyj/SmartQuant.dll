using SmartQuant.Quant;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public  class RandomTest
    {
        [Test]
        public void TestSeeds()
        {
            Assert.AreEqual(9876, Random.Seed1);
            Assert.AreEqual(54321, Random.Seed2);
        }
    }
}

