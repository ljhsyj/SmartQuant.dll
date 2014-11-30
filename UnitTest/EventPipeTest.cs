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
        Framework f = Substitute.For<Framework>("mock", true);

        [SetUp]
        public void SetUp()
        {
            pipe = new EventPipe(f);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void DequeueReturnNull()
        {
            Assert.AreEqual(null, pipe.Dequeue());
            Assert.AreEqual(null, pipe.Read());
        }

        [Test]
        public void TestCount()
        {
            var q1 = new EventQueue(0);
            q1.IsSynched = true;
            var q2 = new EventQueue(0);
            q2.IsSynched = true;
            var q3 = new EventQueue(0);
            q3.IsSynched = false;
            var q4 = new EventQueue(0);
            q3.IsSynched = false;
            pipe.Add(q1);
            pipe.Add(q2);
            pipe.Add(q3);
            pipe.Add(q4);
            Assert.AreEqual(2, pipe.Count);
            var q5 = new EventQueue(0);
            q5.IsSynched = false;
            pipe.Add(q5);
            Assert.AreEqual(3, pipe.Count);
        }

        [Test]
        public void TestSyncedQueueIsEmpty()
        {
            Assert.IsTrue(pipe.IsEmpty());
            var q1 = new EventQueue(0);
            q1.IsSynched = true;
            var q2 = new EventQueue(0);
            q2.IsSynched = true;
            pipe.Add(q1);
            pipe.Add(q2);
            Assert.IsTrue(q1.IsEmpty());
            Assert.IsTrue(q2.IsEmpty());
            Assert.IsTrue(pipe.IsEmpty());

            q1.Enqueue(new Event());
            Assert.IsFalse(q1.IsEmpty());
            Assert.IsTrue(q2.IsEmpty());
            Assert.IsTrue(pipe.IsEmpty());

            q2.Enqueue(new Event());
            Assert.IsFalse(q1.IsEmpty());
            Assert.IsFalse(q2.IsEmpty());
            Assert.IsFalse(pipe.IsEmpty());
        }

        [Test]
        public void TestUnsyncedQueueIsEmpty()
        {
            Assert.IsTrue(pipe.IsEmpty());
            var q1 = new EventQueue(0);
            q1.IsSynched = false;
            var q2 = new EventQueue(0);
            q2.IsSynched = false;
            pipe.Add(q1);
            Assert.IsTrue(pipe.IsEmpty());
            pipe.Add(q2);
            Assert.IsTrue(pipe.IsEmpty());
            Assert.IsTrue(q1.IsEmpty());
            Assert.IsTrue(q2.IsEmpty());
            Assert.IsTrue(pipe.IsEmpty());

            q1.Enqueue(new Event());
            Assert.IsFalse(q1.IsEmpty());
            Assert.IsTrue(q2.IsEmpty());
            Assert.IsFalse(pipe.IsEmpty());

            q2.Enqueue(new Event());
            Assert.IsFalse(q1.IsEmpty());
            Assert.IsFalse(q2.IsEmpty());
            Assert.IsFalse(pipe.IsEmpty());
        }

        [Test]
        public void TestIsEmpty()
        {
            var q1 = new EventQueue(0);
            q1.IsSynched = false;
            pipe.Add(q1);
            Assert.IsTrue(pipe.IsEmpty());
            //  Or
            var q2 = new EventQueue(0);
            q2.IsSynched = true;
            pipe.Add(q2);
            Assert.IsTrue(pipe.IsEmpty());

            q2.Enqueue(new Event());
            Assert.IsFalse(pipe.IsEmpty());
        }
    }
}

