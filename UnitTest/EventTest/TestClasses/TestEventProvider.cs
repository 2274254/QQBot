using LibEvent.Abstracts;
using LibEvent.Attributes;
using System;

namespace UnitTest.EventTest.TestClasses {
    [EventProvider(typeof(TestEvent))]
    public class TestEventProvider : AEventProvider {
        public void NewEvent(Int32 index) {
            this.LaunchEvent(new TestEvent(index));
        }
    }
}
