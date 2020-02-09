using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode
{
    public static class EventBus
    {
        public delegate void EventListener<T>(T customEvent) where T : CustomEvent;

        private static Dictionary<System.Type, IList> eventListeners = new Dictionary<System.Type, IList>();

        public static void Subscribe<T>(EventListener<T> listener) where T : CustomEvent {
            System.Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                eventListeners[type] = new List<EventListener<T>>();
            }
            eventListeners[type].Add(listener);
        }
        public static void Unsubscribe<T>(EventListener<T> listener) where T : CustomEvent {
            System.Type type = typeof(T);
            if (eventListeners.ContainsKey(type) && eventListeners[type].Contains(listener)) {
                eventListeners[type].Remove(listener);
            }
        }

        public static void FireEvent<T>(T customEvent = null) where T : CustomEvent {
            System.Type type = typeof(T);
            if (!eventListeners.ContainsKey(type)) {
                Debug.Log($"No listeners found for the event {type}.");
                return;
            }
            foreach (EventListener<T> listener in eventListeners[type]) {
                listener(customEvent);
            }
        }

        public static void ClearAll() {
            eventListeners.Clear();
        }
    }
}
