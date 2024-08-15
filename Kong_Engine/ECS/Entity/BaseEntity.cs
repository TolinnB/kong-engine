using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/**/
//  Base Entity to create player and enemy entities
// Handles size, texture and systems such as movement and physics can be applied to them
/**/

namespace Kong_Engine.ECS.Entity
{
    public class BaseEntity
    {
        private readonly Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

        protected Texture2D _texture;
        protected Vector2 _position = Vector2.One;

        public int zIndex;

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

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
