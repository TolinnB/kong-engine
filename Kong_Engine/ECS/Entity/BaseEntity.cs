using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.Entity
{
    public class BaseEntity
    {
        private readonly Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

        public void AddComponent<T>(T component) where T : IComponent
        {
            components[typeof(T)] = component;
        }

        public T GetComponent<T>() where T : IComponent
        {
            components.TryGetValue(typeof(T), out var component);
            return (T)component;
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return components.ContainsKey(typeof(T));
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            components.Remove(typeof(T));
        }
    }
}
