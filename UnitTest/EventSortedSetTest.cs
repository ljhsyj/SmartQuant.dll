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
            var t1 = DateTime.Parse("2000/01/01 00:00:01");
            var t2 = DateTime.Parse("2000/01/01 00:00:05");
            var t3 = DateTime.Parse("2000/01/01 00:01:01");
            var t4 = DateTime.Parse("2000/01/01 00:01:01");

            var e1 = new Event() { DateTime = t1 };
            var e2 = new Event() { DateTime = t2 };
            var e3 = new Event() { DateTime = t3 };
            var e4 = new Event() { DateTime = t4 };
            var e5 = new Event() { DateTime = t3 };
            var e6 = new Event() { DateTime = t2 };

            var sset = new EventSortedSet();
            sset.Add(e1);
            sset.Add(e2);
            sset.Add(e3);
            sset.Add(e4);
            sset.Add(e5);
            Assert.AreEqual(5, sset.Count);
            Assert.AreSame(e1, sset[0]);
            Assert.AreSame(e2, sset[1]);
            Assert.AreSame(e3, sset[2]);
            Assert.AreSame(e4, sset[3]);
            Assert.AreSame(e5, sset[4]);
            sset.Add(e6);
            Assert.AreEqual(6, sset.Count);
            Assert.AreSame(e1, sset[0]);
            Assert.AreSame(e2, sset[1]);
            Assert.AreSame(e6, sset[2]);
            Assert.AreSame(e3, sset[3]);
            Assert.AreSame(e4, sset[4]);
            Assert.AreSame(e5, sset[5]);
        }
    }
}

