using System;
using System.Collections.Generic;
using System.Text;
using ChatBot.Event.Enums;

namespace ChatBot.Event {
    public class EventHandleAttribute {
        public EventPriority Priority { get; private set; }

        public EventHandleAttribute(EventPriority priority) {
            this.Priority = priority;
        }
    }
}
