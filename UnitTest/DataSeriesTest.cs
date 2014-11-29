using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class DataSeriesTest
    {
        [Test]
        public void TestDataSeries()
        {
            var f = Framework.Current;
            var ds = f.DataServer.GetDataSeries("AAPL.0.Trade");
            Console.WriteLine("{0} - {1}", ds.DateTime1, ds.DateTime2);
        }
    }
}

