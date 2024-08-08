using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Component;
using System.Diagnostics;

namespace Kong_Engine.ECS.Entity
{
    public class Asteroid : BaseEntity
    {
        private Texture2D _texture;
        private Rectangle defaultFrame;
        private Rectangle breakingFrame1;
        private Rectangle breakingFrame2;
        private float _scale;
        private Vector2 _velocity;
        private Rectangle asteroidBounds; // For collisions

        private int frameWidth = 50; // Width of each frame
        private int frameHeight = 50; // Height of each frame

        public Asteroid(Texture2D texture, Vector2 position, float scale, Vector2 velocity)
        {
            _texture = texture;
            _scale = scale;
            _velocity = velocity;

            // Define the source rectangles for each frame
            defaultFrame = new Rectangle(0, 248, frameWidth, frameHeight);    // Default frame
            breakingFrame1 = new Rectangle(50, 0, frameWidth, frameHeight); // Breaking state frame 1
            breakingFrame2 = new Rectangle(100, 0, frameWidth, frameHeight);// Breaking state frame 2

            AddComponent(new PositionComponent { Position = position });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale))
            });

            asteroidBounds = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale));

            // Log initial position and texture status
            Debug.WriteLine($"Asteroid initialized at position {position}, texture loaded: {_texture != null}");
        }

        public void Update(GameTime gameTime)
        {
            var positionComponent = GetComponent<PositionComponent>();
            positionComponent.Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update bounding box
            asteroidBounds.X = (int)positionComponent.Position.X;
            asteroidBounds.Y = (int)positionComponent.Position.Y;

            // Update collision component bounding box
            var collisionComponent = GetComponent<CollisionComponent>();
            if (collisionComponent != null)
            {
                collisionComponent.BoundingBox = asteroidBounds;
            }

            // Log bounding box position for debugging
            Debug.WriteLine($"Asteroid bounding box: {asteroidBounds}");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var positionComponent = GetComponent<PositionComponent>();
            var position = positionComponent.Position;

            // You can switch frames based on the asteroid's state or other conditions here
            Rectangle currentFrame = defaultFrame; // You can change this logic to choose between frames

            // Log draw call
            Debug.WriteLine($"Drawing asteroid at position {position}");

            spriteBatch.Draw(_texture, position, currentFrame, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }
    }
}
