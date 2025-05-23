﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Component;
using System.Diagnostics;

namespace Kong_Engine.ECS.Entity
{
    public class Asteroid : BaseEntity
    {
        private new Texture2D _texture;
        private Rectangle defaultFrame;
        private Rectangle[] explosionFrames;
        private float _scale;
        private Vector2 _velocity;
        private Rectangle asteroidBounds; // For collisions
        private bool isExploding = false;
        private float explosionFrameTime = 0.1f; // Time per frame
        private int currentExplosionFrame = 0;
        private double explosionElapsedTime = 0;

        private int frameWidth = 50; // Width of each frame
        private int frameHeight = 50; // Height of each frame

        public bool MarkedForRemoval { get; private set; } = false;

        private AudioManager _audioManager;

        public Asteroid(Texture2D texture, Vector2 position, float scale, Vector2 velocity, AudioManager audioManager)
        {
            _texture = texture;
            _scale = scale;
            _velocity = velocity;
            _audioManager = audioManager;

            // Define the source rectangles for each frame
            defaultFrame = new Rectangle(0, 248, frameWidth, frameHeight);
            explosionFrames = new Rectangle[4];
            explosionFrames[0] = new Rectangle(40, 155, 28, 30);
            explosionFrames[1] = new Rectangle(122, 260, 28, 30);
            explosionFrames[2] = new Rectangle(159, 260, 28, 30);
            explosionFrames[3] = new Rectangle(195, 260, 28, 30);

            AddComponent(new PositionComponent { Position = position });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale))
            });

            asteroidBounds = new Rectangle((int)position.X, (int)position.Y, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }


        public void Update(GameTime gameTime)
        {
            var positionComponent = GetComponent<PositionComponent>();
            if (isExploding)
            {
                explosionElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (explosionElapsedTime >= explosionFrameTime)
                {
                    explosionElapsedTime = 0;
                    currentExplosionFrame++;
                    if (currentExplosionFrame >= explosionFrames.Length)
                    {
                        // Remove the asteroid from the game
                        MarkForRemoval();
                    }
                }
            }
            else
            {
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
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var positionComponent = GetComponent<PositionComponent>();
            var position = positionComponent.Position;

            Rectangle currentFrame = defaultFrame; // Default frame if not exploding
            if (isExploding)
            {
                currentFrame = explosionFrames[currentExplosionFrame];
            }

            spriteBatch.Draw(_texture, position, currentFrame, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void StartExplosion()
        {
            isExploding = true;
            currentExplosionFrame = 0;
            explosionElapsedTime = 0;

            // Play explosion sound
            _audioManager.PlaySound("explosion");
        }


        private void MarkForRemoval()
        {
            MarkedForRemoval = true;
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }
    }
}
