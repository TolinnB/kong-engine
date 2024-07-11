using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.ECS.Component
{
    public interface IComponent
    {
        // Marker interface for components
    }

    public class PositionComponent : IComponent
    {
        public Vector2 Position { get; set; }
    }

    public class TextureComponent : IComponent
    {
        public Texture2D Texture { get; set; }
    }
}
