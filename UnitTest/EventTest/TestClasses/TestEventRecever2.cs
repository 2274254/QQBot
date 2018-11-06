using LibEvent.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UnitTest.EventTest.TestClasses {
    class TestEventRecever2 {
        public static Int32 TestEventRecever2Result { get; private set; }
        public static Int32 TestEventRecever2TID { get; private set; }

        [EventHandle]
        public void OnTestEvent2(TestEvent @event) {
            TestEventRecever2Result = @event.Index;
            TestEventRecever2TID = Thread.CurrentThread.ManagedThreadId;
        }
    }
}
