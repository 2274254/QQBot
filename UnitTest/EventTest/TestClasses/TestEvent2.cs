using LibEvent.Interfaces;
using System;

namespace UnitTest.EventTest.TestClasses {
    public class TestEvent2 : IEvent {
        public Int32 Index { get; private set; }
        public TestEvent2(Int32 index) {
            this.Index = index;
        }
    }
}
