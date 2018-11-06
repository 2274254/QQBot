using LibEvent.Enums;
using System;
using System.Collections.Concurrent;

namespace LibEvent.Structs {
    public class EventRegisterInfo {
        public readonly Type EventType;
        public readonly EventPriority Priority;
        public ConcurrentBag<EventHandleInfo> EventHandleList = new ConcurrentBag<EventHandleInfo>();

        public EventRegisterInfo(Type eventType, EventPriority priority) {
            this.EventType = eventType;
            this.Priority = priority;
        }
    }
}
