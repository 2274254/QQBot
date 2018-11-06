using LibEvent.Enums;
using System;

namespace LibEvent.Attributes {
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandle : Attribute {
        public EventPriority Priority { get; set; } = EventPriority.NORMAL;
    }
}