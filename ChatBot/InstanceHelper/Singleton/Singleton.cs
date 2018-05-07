using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ChatBot.InstanceHelper.Singleton {
    public class Singleton<T> {
        private static Dictionary<Type, Object> TypeObjectMap = new Dictionary<Type, Object>();
        private static List<Type> TypeLoadList = new List<Type>();

        static public T Instance { get => GetInstance(typeof(T)); }

        public Singleton() {
            lock (TypeLoadList) {
                if (!TypeLoadList.Contains(this.GetType())) {
                    throw new NewSingletonClassException(String.Format("Class {0} is Singleton, Use \"{1}.Instance;\" but \"new {1}();\"", this.GetType().FullName, this.GetType().Name));
                }
            }
        }

        private static void LockUnregisterTypeLoadStatus(Type _Type) {
            lock (TypeLoadList) {
                if (TypeLoadList.Contains(_Type)) {
                    TypeLoadList.Remove(_Type);
                }
            }
        }

        private static void LockRegisterTypeLoadStatus(Type _Type) {
            lock (TypeLoadList) {
                if (!TypeLoadList.Contains(_Type)) {
                    TypeLoadList.Add(_Type);
                }
            }
        }

        protected static T GetInstance(Type _Type) {
            if (!TypeObjectMap.ContainsKey(_Type)) {
                lock (TypeObjectMap) {
                    if (!TypeObjectMap.ContainsKey(_Type)) {
                        LockRegisterTypeLoadStatus(_Type);
                        var Instance = Activator.CreateInstance(_Type);
                        LockUnregisterTypeLoadStatus(_Type);
                        TypeObjectMap.Add(_Type, Instance);
                    }
                }
            }
            return (T)TypeObjectMap.GetValueOrDefault(_Type);
        }
    }
}
