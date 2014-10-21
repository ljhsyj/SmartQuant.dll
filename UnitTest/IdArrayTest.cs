using System;
using NUnit.Framework;
using SmartQuant;
namespace UnitTest
{
    [TestFixture]
    public class IdArrayTest
    {
        [Test]
        public void TestResize()
        {
            var arr = new IdArray<double>(10);
            Assert.AreEqual(arr.Size, 10);
            Assert.DoesNotThrow(() => arr[15] = 20.0);
        }
    }
}

