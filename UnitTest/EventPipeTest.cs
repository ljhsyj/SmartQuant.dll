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
        //   pipe = new EventPipe(f);
        }
        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void DequeueReturnNull()
        {
            //var q = Substitute.For<IEventQueue>();
            Assert.AreEqual(null, pipe.Dequeue());
        }
    }
}

