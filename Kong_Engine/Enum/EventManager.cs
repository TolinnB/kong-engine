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

        /// Allows a listener to subscribe to a particular event, adding them to a list
        public static void Subscribe(Events eventType, Action<object> listener)
        {
            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Action<object>>();
            }
            eventListeners[eventType].Add(listener);
        }

        /// Allows a listener to unsubscribe to an event, removing them from a list
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

        /// This triggers an event and then tells anything in the subscribe list that it has happened
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
