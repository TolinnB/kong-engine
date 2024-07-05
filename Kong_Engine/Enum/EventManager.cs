using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kong_Engine.Enum;

namespace Kong_Engine
{
    public static class EventManager
    {
        private static Dictionary<Events, List<Action<object>>> eventListeners = new Dictionary<Events, List<Action<object>>>();

        /// Allows a listener to subscribe to a particular event
        public static void Subscribe(Events eventType, Action<object> listener)
        {
            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Action<object>>();
            }
            eventListeners[eventType].Add(listener);
        }

        /// Allows a listener to unsubscribe to an event
        public static void Unsubscribe(Events eventType, Action<object> listener)
        {
            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(listener);
                if (eventListeners[eventType].Count == 0)
                {
                    eventListeners.Remove(eventType);
                }
            }
        }

        /// Even trigger, and then tells each listener the event has taken place
        public static void TriggerEvent(Events eventType, object argument = null)
        {
            if (eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in eventListeners[eventType])
                {
                    listener?.Invoke(argument);
                }
            }
        }
    }
}
