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
          //  pipe = new EventPipe(Fr);
        }
        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void DequeueReturnNull()
        {
         //   var pipe = new EventPipe(f);
            Assert.AreEqual(null, pipe.Dequeue());
        }
    }
}

