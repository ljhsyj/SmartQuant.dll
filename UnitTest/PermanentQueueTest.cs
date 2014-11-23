using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    class Reader
    {
    }
    [TestFixture]
    public class PermanentQueueTest
    {
        [Test()]
        public void TestCase()
        {
            var reader = new Reader();
            var q = new PermanentQueue<object>();
            q.AddReader(reader);
            var obj1 = new object();
            q.Enqueue(obj1);
            q.Enqueue(new object());
            var all = q.DequeueAll(reader);
            Assert.AreSame(all[0], obj1);
        }
    }
}