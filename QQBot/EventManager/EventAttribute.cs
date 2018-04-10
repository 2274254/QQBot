using System;

namespace QQBot.EventManager {
    internal class EventAttribute : Attribute {
        private EventPriority priority { get; }

        public EventAttribute(EventPriority priority) {
            this.priority = priority;
        }
    }
}