using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.System;

/**/
// Base component for ECS architecture
// Represents different parts of an entity's behaviour, such as position, texture, movement
// collision boundaries, lives, and physics.
// This can be exended to create your own entities and physics components
/**/

namespace Kong_Engine.ECS.Component
{
    public interface IComponent
    {
        // Marker interface
    }

    public class PositionComponent : IComponent
    {
        public Vector2 Position { get; set; }
    }

    public class TextureComponent : IComponent
    {
        public Texture2D Texture { get; set; }
    }

    public class MovementComponent : IComponent
    {
        public Vector2 Velocity { get; set; }
        public float LeftBoundary { get; set; }
        public float RightBoundary { get; set; }
        public bool MovingRight { get; set; }
    }

    public class CollisionComponent : IComponent
    {
        public Rectangle BoundingBox { get; set; }
    }

    public class LifeComponent : IComponent
    {
        public int Lives { get; set; }
    }

    public class PhysicsComponent : IComponent
    {
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }
        public bool IsGrounded { get; set; }
    }
}
