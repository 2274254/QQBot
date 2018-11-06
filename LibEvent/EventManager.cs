using Kontur.Cache.ConcurrentPriorityQueue;
using LibEvent.Attributes;
using LibEvent.Interfaces;
using LibEvent.Structs;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace LibEvent {
    public class EventManager {
        private readonly EventSender Sender;
        private readonly ConcurrentDictionary<Type, EventRegisterInfo> RegisterInfo = new ConcurrentDictionary<Type, EventRegisterInfo>();
        private readonly ConcurrentBag<Object> LoadedRecever = new ConcurrentBag<Object>();
        private readonly PriorityQueue<IEvent> EventQueue = new PriorityQueue<IEvent>();
        private readonly Thread DispatchThread;
        private Boolean StopDispatch { get; set; } = false;

        public Boolean RegisterEventProvider(Object @object) {
            if (@object is IEventProvider) {
                var attribute = (EventProvider)@object.GetType().GetCustomAttribute(typeof(EventProvider), false);
                if (attribute == null) {
                    return false;
                }

                if (this.RegisterInfo.ContainsKey(attribute.EventProvided)) {
                    return false;
                }

                this.RegisterInfo.TryAdd(attribute.EventProvided, new EventRegisterInfo(attribute.EventProvided, attribute.Priority));

                try {
                    ((IEventProvider)@object).InitizeEventProvider(this.Sender);
                    return true;
                } catch (Exception) {
                    return false;
                }
            } else {
                return false;
            }
        }

        private Object GetReceverInstance(Type type) {
            foreach (var loaded in this.LoadedRecever) {
                if (loaded.GetType().Equals(type)) {
                    return loaded;
                }
            }

            try {
                var instance = Activator.CreateInstance(type);
                this.LoadedRecever.Add(instance);
                return instance;
            } catch {
                return null;
            }
        }

        public void RegisterEventRecever(Assembly assembly) {
            foreach (Type type in assembly.GetTypes()) {
                foreach (MethodInfo method in type.GetMethods()) {
                    var attribute = (EventHandle)method.GetCustomAttribute(typeof(EventHandle), false);

                    if (attribute == null) {
                        continue;
                    }

                    ParameterInfo[] parameters = method.GetParameters();

                    if (parameters.Length != 1) {
                        continue;
                    }

                    Type parametersType = parameters[0].ParameterType;

                    if (this.RegisterInfo.TryGetValue(parametersType, out EventRegisterInfo registered)) {
                        var instance = this.GetReceverInstance(type);
                        if (instance != null) {
                            registered.EventHandleList.Add(new EventHandleInfo(method, instance, attribute.Priority));
                        }
                    }
                }
            }
        }

        private void DispatchThreadMethod() {
            var snipWait = new SpinWait();

            while (!this.StopDispatch) {
                if (this.EventQueue.Length == 0) {
                    snipWait.SpinOnce();
                } else {
                    snipWait.Reset();

                    IEvent @event = this.EventQueue.Dequeue();
                    var pars = new Object[] { @event };
                    var subeventQueue = new PriorityQueue<EventHandleInfo>();

                    if (this.RegisterInfo.TryGetValue(@event.GetType(), out EventRegisterInfo registered)) {
                        foreach (EventHandleInfo handle in registered.EventHandleList) {
                            subeventQueue.Add(handle, (Int32)handle.Priority);
                        }

                        while (subeventQueue.Length != 0) {
                            EventHandleInfo handle = subeventQueue.Dequeue();
                            ThreadPool.QueueUserWorkItem(state => handle.Method.Invoke(handle.Instance, pars));
                        }
                    }
                }
            }
        }

        internal void DispatchEvent(IEvent @event) {
            if (this.RegisterInfo.TryGetValue(@event.GetType(), out EventRegisterInfo registered)) {
                this.EventQueue.Add(@event, (Int32)registered.Priority);
            }
        }

        public EventManager() {
            this.DispatchThread = new Thread(new ThreadStart(this.DispatchThreadMethod)) {
                IsBackground = false
            };
            this.Sender = new EventSender(this);
            this.DispatchThread.Start();
        }

        ~EventManager() {
            this.StopDispatch = true;

            var spinWait = new SpinWait();

            while (spinWait.Count < 20) {
                if (!this.DispatchThread.IsAlive) {
                    break;
                }
            }

            if (this.DispatchThread.IsAlive) {
                try { this.DispatchThread.Abort(); } catch { }
            }
        }
    }
}
