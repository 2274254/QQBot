using LibEvent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Threading;
using UnitTest.EventTest.TestClasses;

namespace UnitTest.EventTest {
    [TestClass]
    public class LibEventTest {
        [TestMethod]
        public void BasicEventTest() {
            var eventManager = new EventManager();
            var provider = new TestEventProvider();

            eventManager.RegisterEventProvider(provider);
            eventManager.RegisterEventRecever(Assembly.GetExecutingAssembly());

            provider.NewEvent(1);

            Thread.Sleep(50);

            Assert.AreEqual(1, TestEventRecever.TestEventReceverResult);
            Assert.AreEqual(1, TestEventRecever2.TestEventRecever2Result);
        }

        [TestMethod]
        public void DifferentEventTest() {
            var eventManager = new EventManager();
            var provider = new TestEventProvider();
            var provider2 = new TestEventProvider2();

            eventManager.RegisterEventProvider(provider);
            eventManager.RegisterEventProvider(provider2);
            eventManager.RegisterEventRecever(Assembly.GetExecutingAssembly());

            provider.NewEvent(1);
            provider2.NewEvent(2);

            Thread.Sleep(50);

            Assert.AreEqual(1, TestEventRecever.TestEventReceverResult);
            Assert.AreEqual(2, TestEventRecever.TestEventRecever3Result);
        }
    }
}
