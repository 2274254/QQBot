using LibEvent.Interfaces;
using System;

namespace UnitTest.EventTest.TestClasses {
    public class TestEvent : IEvent {
        public Int32 Index { get; private set; }
        public TestEvent(Int32 index) {
            this.Index = index;
        }
    }
}
