using LibEvent.Interfaces;

namespace LibEvent {
    public class EventSender {
        private readonly EventManager EventManager;

        public EventSender(EventManager eventManager) {
            this.EventManager = eventManager;
        }

        public void LaunchEvent(IEvent @event) {
            this.EventManager.DispatchEvent(@event);
        }
    }
}
