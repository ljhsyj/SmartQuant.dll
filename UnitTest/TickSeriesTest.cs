using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class TickSeriesTest
    {
        [Test]
        public void TestGetIndex()
        {
            var ts = new TickSeries("test");
            for (int i = 0; i < 10; ++i)
                ts.Add(new Tick{ DateTime = new DateTime(2000, 1, 1, 10, i, 30) });
          
            var firstDt = new DateTime(2000, 1, 1, 10, 3, 30);
            var firstTick = new Tick { DateTime = firstDt };
            var lastDt = new DateTime(2000, 1, 1, 10, 9, 30);
            var lastTick = new Tick { DateTime = lastDt };

            // DateTime is in the middle;
            Assert.AreEqual(3, ts.GetIndex(firstDt, IndexOption.Null));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 25), IndexOption.Null));
            Assert.AreEqual(4, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 30), IndexOption.Null));
            Assert.AreEqual(4, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 30), IndexOption.Prev));
            Assert.AreEqual(4, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 30), IndexOption.Next));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 25), IndexOption.Null));
            Assert.AreEqual(3, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 25), IndexOption.Prev));
            Assert.AreEqual(4, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 25), IndexOption.Next));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 40), IndexOption.Null));
            Assert.AreEqual(4, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 40), IndexOption.Prev));
            Assert.AreEqual(5, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 40), IndexOption.Next));

            // DateTime > LastDateTime
            Assert.AreEqual(5, ts.GetIndex(new DateTime(2000, 1, 1, 10, 4, 40), IndexOption.Next));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 10, 11, 30), IndexOption.Null));
            Assert.AreEqual(9, ts.GetIndex(new DateTime(2000, 1, 1, 10, 11, 30), IndexOption.Prev));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 10, 11, 30), IndexOption.Next));

            // DateTime < FirstDateTime
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 9, 31, 30), IndexOption.Null));
            Assert.AreEqual(-1, ts.GetIndex(new DateTime(2000, 1, 1, 9, 31, 30), IndexOption.Prev));
            Assert.AreEqual(0, ts.GetIndex(new DateTime(2000, 1, 1, 9, 31, 30), IndexOption.Next));
        }
    }
}

