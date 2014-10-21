using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class EventSortedSetTest
    {
        [Test()]
        public void TestOrder()
        {
            var t1 = DateTime.Parse("2000/01/01 00:00:02");
            var t2 = DateTime.Parse("2000/01/01 00:00:01");
            var t3 = DateTime.Parse("2000/01/01 00:01:01");
            Assert.IsTrue(t2 < t1);
            Assert.IsTrue(t3 > t1);
            Assert.IsTrue(t3 > t2);
            var e1 = new Event() { DateTime = t1 };
            var e2 = new Event() { DateTime = t2 };
            var e3 = new Event() { DateTime = t3 };
            var sset = new EventSortedSet();
            sset.Add(e1);
            sset.Add(e2);
            sset.Add(e3);
            Assert.IsTrue(sset[0] == e2);
            Assert.IsTrue(sset[1] == e1);
            Assert.IsTrue(sset[2] == e3);
        }
    }
}

