using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class EventQueueTest
    {
    }

    [TestFixture]
    public class SortedEventQueueTest
    {
        [Test]
        public void TestApiBehaviors()
        {
            var q = new SortedEventQueue(EventQueueId.All);
            Assert.Throws(typeof(NotImplementedException), () => Console.WriteLine(q.EmptyCount));
            Assert.Throws(typeof(NotImplementedException), () => Console.WriteLine(q.FullCount));
            Assert.Throws(typeof(NotImplementedException), () => q.Write(null));
            Assert.IsFalse(q.IsSynched);
        }
    }
}

