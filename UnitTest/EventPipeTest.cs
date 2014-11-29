using System;
using NUnit.Framework;
using SmartQuant;
using NSubstitute;

namespace UnitTest
{
    [TestFixture]
    public class EventPipeTest
    {
        EventPipe pipe;

        [SetUp]
        public void SetUp()
        {
            var f = Substitute.For<Framework>("mock", true);
            pipe = new EventPipe(f);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void DequeueReturnNull()
        {
            Assert.AreEqual(null, pipe.Dequeue());
            Assert.AreEqual(null, pipe.Read());
            Assert.IsTrue(pipe.IsEmpty());

        }
    }
}

