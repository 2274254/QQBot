using LibEvent.Abstracts;
using LibEvent.Attributes;
using System;

namespace UnitTest.EventTest.TestClasses {
    [EventProvider(typeof(TestEvent2))]
    public class TestEventProvider2 : AEventProvider {
        public void NewEvent(Int32 index) {
            this.LaunchEvent(new TestEvent2(index));
        }
    }
}
