using System;
using NUnit;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class EventManagerTest
    {
        [Test]
        public void TestAtStartup()
        {
            var f = Framework.Current;
            var manager = new EventManager(f, f.EventBus);
            // Running at Startup
            Assert.AreEqual(manager.Status, EventManagerStatus.Running);
        }
    }
}

