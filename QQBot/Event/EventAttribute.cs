using System;

namespace QQBot.Event {
    internal class EventAttribute : Attribute {
        private EventPriority priority { get; }

        public EventAttribute(EventPriority priority) {
            this.priority = priority;
        }
    }
}