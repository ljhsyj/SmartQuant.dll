using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
	public class MemorySeriesTest
    {
        [Test]
        public void TestGetIndex()
        {
            var dt1 = DateTime.Parse("2000/01/01 12:30:00");
            var dt2 = DateTime.Parse("2000/01/01 12:31:00");
            var dt3 = DateTime.Parse("2000/01/01 12:32:00");
            var dt4 = DateTime.Parse("2000/01/01 12:33:00");
            var dt5 = DateTime.Parse("2000/01/01 12:34:00");

            var ts = new MemorySeries();
            ts.Add(dt1, 10);
            ts.Add(dt2, 11);
            ts.Add(dt3, 9);
            ts.Add(dt4, 15);
            ts.Add(dt5, 11);

            Assert.AreEqual(1, ts.GetIndex(dt2, SearchOption.ExactFirst));
            Assert.AreEqual(1, ts.GetIndex(dt2, SearchOption.Next));
            Assert.AreEqual(1, ts.GetIndex(dt2, SearchOption.Prev));

            var dtBeforeFirstDt = DateTime.Parse("2000/01/01 12:29:11");
            var dtAfterFirstDt = DateTime.Parse("2000/01/01 12:35:23");
            var dtMiddle =  DateTime.Parse("2000/01/01 12:33:54");
            Assert.AreEqual(-1, ts.GetIndex(dtBeforeFirstDt, SearchOption.ExactFirst));
            Assert.AreEqual(0, ts.GetIndex(dtBeforeFirstDt, SearchOption.Next));
            Assert.AreEqual(-1, ts.GetIndex(dtBeforeFirstDt, SearchOption.Prev));

            Assert.AreEqual(-1, ts.GetIndex(dtAfterFirstDt, SearchOption.ExactFirst));
            Assert.AreEqual(-1, ts.GetIndex(dtAfterFirstDt, SearchOption.Next));
            Assert.AreEqual(ts.Count-1, ts.GetIndex(dtAfterFirstDt, SearchOption.Prev));

            Assert.AreEqual(-1, ts.GetIndex(dtMiddle, SearchOption.ExactFirst));
            Assert.AreEqual(3, ts.GetIndex(dtMiddle, SearchOption.Prev));
            Assert.AreEqual(4, ts.GetIndex(dtMiddle, SearchOption.Next));
        }
    }
}