using LibEvent.Attributes;
using LibEvent.Enums;
using System;
using System.Threading;

namespace UnitTest.EventTest.TestClasses {

    public class TestEventRecever {
        public static Int32 TestEventReceverResult { get; private set; }
        public static Int32 TestEventReceverTID { get; private set; }

        public static Int32 TestEventRecever3Result { get; private set; }
        public static Int32 TestEventRecever3TID { get; private set; }

        [EventHandle(Priority = EventPriority.HIGHEST)]
        public void OnTestEvent(TestEvent @event) {
            TestEventReceverResult = @event.Index;
            TestEventReceverTID = Thread.CurrentThread.ManagedThreadId;
        }

        [EventHandle(Priority = EventPriority.LOW)]
        public void OnTestEvent3(TestEvent2 @event) {
            TestEventRecever3Result = @event.Index;
            TestEventRecever3TID = Thread.CurrentThread.ManagedThreadId;
        }
    }
}
