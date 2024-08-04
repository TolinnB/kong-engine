using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.System;

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

    public class ParticleComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;
        public float Size;
        public float LifeTime;

        public ParticleComponent(Vector2 position, Vector2 velocity, Color color, float size, float lifeTime)
        {
            Position = position;
            Velocity = velocity;
            Color = color;
            Size = size;
            LifeTime = lifeTime;
        }

        public void Update(GameTime gameTime)
        {
            LifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (LifeTime > 0)
            {
                spriteBatch.Draw(texture, Position, null, Color, 0f, Vector2.Zero, Size, SpriteEffects.None, 0f);
            }
        }
    }
}
