using System;
using NUnit.Framework;
using System.Text;
using System.IO;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class DataFileTest
    {
        [Test]
        public void TestInstrumentFile()
        {
            var file = Configuration.DefaultConfiguaration().InstrumentFileName;
            var sm = new StreamerManager();
            sm.Add(new InstrumentStreamer());
            var df = new DataFile(file, sm);
            df.Open();
            foreach (var key in df.Keys.Values)
            { 
                if (key.TypeId == ObjectType.Instrument)
                {
                    var instrument = key.GetObject() as Instrument;
                    Console.WriteLine(instrument);
                }
            }
        }

        [Test]
        public void TestDataFile()
        {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SmartQuant Ltd", "OpenQuant 2014", "data", "data.quant");
            var sm = new StreamerManager();
            sm.Add(new AskStreamer());
            sm.Add(new ObjectStreamer());
            var df = new DataFile(file, sm);
            df.Open();
//            df.DumpHeader();
//            foreach (var key in df.Keys.Values)
//                key.DumpDetail();
            DataSeries ds = null;
            ObjectKey dsKey = null;

            foreach (var key in df.Keys.Values)
            {
                if (key.TypeId == ObjectType.DataSeries)
                {
                    var obj = df.Get(key.name);
                    if (dsKey == null)
                    {
                        dsKey = key;
                        ds = (DataSeries)obj;
                    }
//                    Console.WriteLine(((DataSeries)obj).Name);
//                    Console.WriteLine(((DataSeries)obj).Count);
                }
            }
            ds.Dump();
            object obj1 = ds.Get(2);
            if (obj1 is Ask)
            {
                var ask = obj1 as Ask;
                Console.WriteLine("dt:{0}", ask.DateTime);
                Console.WriteLine("price:{0}", ask.Price);
                Console.WriteLine("size:{0}", ask.Size);
            }
            df.Close();
        }


        [Test]
        public void TestLabelBinaryLength()
        {
            Assert.AreEqual(11, GetStringBinaryStreamLength("SmartQuant"));
            Assert.AreEqual(5, GetStringBinaryStreamLength("OKey"));
            Assert.AreEqual(5, GetStringBinaryStreamLength("DKey"));
            Assert.AreEqual(5, GetStringBinaryStreamLength("FKey"));
        }

        private long GetStringBinaryStreamLength(string s)
        {
            long len;
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(s);
                    len = writer.BaseStream.Position;
                }
            }
            return len;
        }
    }
}

