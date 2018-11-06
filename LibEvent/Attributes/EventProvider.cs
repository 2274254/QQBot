using LibEvent.Enums;
using System;

namespace LibEvent.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class EventProvider : Attribute {
        public readonly Type EventProvided;
        public EventPriority Priority { get; set; } = EventPriority.NORMAL;
        public EventProvider(Type eventProvided) {
            this.EventProvided = eventProvided;
        }
    }
}
