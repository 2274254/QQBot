using ChatBotFramework.Event.Enums;

namespace ChatBotFramework.Event {
    public class EventHandleAttribute {
        public EventPriority Priority { get; private set; }

        public EventHandleAttribute(EventPriority priority) {
            this.Priority = priority;
        }
    }
}
