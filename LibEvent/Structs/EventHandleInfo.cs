using LibEvent.Enums;
using System;
using System.Reflection;

namespace LibEvent.Structs {
    public class EventHandleInfo {
        public readonly MethodInfo Method;
        public readonly Object Instance;
        public readonly EventPriority Priority;

        public EventHandleInfo(MethodInfo method, Object instance, EventPriority priority) {
            this.Method = method;
            this.Instance = instance;
            this.Priority = priority;
        }
    }
}
