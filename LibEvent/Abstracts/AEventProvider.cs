using LibEvent.Interfaces;

namespace LibEvent.Abstracts {
    public abstract class AEventProvider : IEventProvider {
        private EventSender _sender;
        public void InitizeEventProvider(EventSender eventSender) {
            this._sender = eventSender;
        }

        protected void LaunchEvent(IEvent @event) {
            this._sender.LaunchEvent(@event);
        }
    }
}
