using System;
using SmartQuant;
using NUnit.Framework;

namespace UnitTest
{
    class Me
    {
        private static int cnt = 0;
        private int id;
        public string Name { get; set; }
        public Me(string name)
        {
            id = cnt++;
            Name = name;

        }

        public void SetId(int id)
        {
            this.id = id;
        }

        internal int GetId()
        {
            return id;
        }

        internal string GetName()
        {
            return Name;
        }
    }
    [TestFixture]
    public class GetByListTest
    {
        [Test()]
        public void TestGetMethods()
        {
            var list = new GetByList<Me>();
            var firstme = new Me("first");
            list.Add(firstme);
            var secondme = new Me("second");
            secondme.SetId(10);
            list.Add(secondme);
            Assert.AreEqual(2, list.Count);
            Assert.AreSame(secondme, list.GetById(10));
            Assert.AreSame(firstme, list.GetByName("first"));
            Assert.AreSame(firstme, list.GetByIndex(0));
            Assert.AreSame(secondme, list.GetByIndex(1));
        }
    }
}

