using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Kong_Engine.Objects
{
    public class RaccoonSprite : BaseEntity
    {
        public Vector2 Knockback { get; set; }
        private Texture2D spriteSheet;
        private Rectangle defaultFrame;
        private Rectangle leftTurnFrame;
        private Rectangle rightTurnFrame;
        private float moveSpeed = 1.5f;
        private Rectangle playerBounds; // For collisions
        private bool isFacingRight = true; // Flag to check direction
        private float scale;
        private int frameWidth = 42; // Width of each frame
        private int frameHeight = 43; // Height of each frame

        private bool isHit = false;
        private double hitTimer = 0;
        public int Score { get; private set; } // Score property
        private readonly AudioManager _audioManager;

        public RaccoonSprite(Texture2D spriteSheet, float scale, AudioManager audioManager)
        {
            this.spriteSheet = spriteSheet;
            this.scale = scale;
            this._audioManager = audioManager;

            // Define the source rectangles for each frame
            defaultFrame = new Rectangle(40, 0, frameWidth, frameHeight);   // Default standing frame
            leftTurnFrame = new Rectangle(0, 0, frameWidth, frameHeight);  // Turning left frame
            rightTurnFrame = new Rectangle(82, 0, frameWidth, frameHeight); // Turning right frame

            AddComponent(new PositionComponent { Position = new Vector2(650, 625) }); // Start position set here
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, (int)(frameWidth * scale), (int)(frameHeight * scale))
            });
            AddComponent(new LifeComponent { Lives = 3 });
            Knockback = Vector2.Zero;

            playerBounds = new Rectangle((int)GetComponent<PositionComponent>().Position.X - 8, (int)GetComponent<PositionComponent>().Position.Y - 8, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public void Update(GameTime gameTime)
        {
            ApplyKnockback();
            HandleInput(gameTime);

            var position = GetComponent<PositionComponent>().Position;
            playerBounds.X = (int)position.X - 8;
            playerBounds.Y = (int)position.Y - 8;

            // Update collision component bounding box
            var collisionComponent = GetComponent<CollisionComponent>();
            if (collisionComponent != null)
            {
                collisionComponent.BoundingBox = playerBounds;
            }

            // Handle hit flashing
            if (isHit)
            {
                hitTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (hitTimer > 0.5)
                {
                    isHit = false;
                    hitTimer = 0;
                }
            }
        }

        public void HandleCollision(Vector2 knockbackForce)
        {
            var lifeComponent = GetComponent<LifeComponent>();
            if (lifeComponent != null)
            {
                lifeComponent.Lives--;
            }
            isHit = true;
            hitTimer = 0;
            Knockback = knockbackForce;
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }

        private void ApplyKnockback()
        {
            if (Knockback != Vector2.Zero)
            {
                var currentPosition = GetComponent<PositionComponent>().Position;
                currentPosition += Knockback;
                Knockback *= 0.9f; // Decay the knockback over time

                if (Knockback.LengthSquared() < 0.01f)
                {
                    Knockback = Vector2.Zero;
                }

                var positionComponent = GetComponent<PositionComponent>();
                positionComponent.Position = currentPosition;
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            Vector2 movement = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= moveSpeed;
                isFacingRight = false; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += moveSpeed;
                isFacingRight = true; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                movement.Y -= moveSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y += moveSpeed;
            }

            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition += movement * moveSpeed;

            var positionComponent = GetComponent<PositionComponent>();
            positionComponent.Position = currentPosition;
        }

        public void Move(Vector2 direction)
        {
            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition += direction;
            var positionComponent = GetComponent<PositionComponent>();
            positionComponent.Position = currentPosition;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle currentFrameRect;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                currentFrameRect = leftTurnFrame;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                currentFrameRect = rightTurnFrame;
            }
            else
            {
                currentFrameRect = defaultFrame;
            }

            var position = GetComponent<PositionComponent>().Position;

            // Flip the sprite if facing left
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Flash red when hit
            Color drawColor = isHit ? Color.Red : Color.White;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, drawColor, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }

        public bool IsDead()
        {
            var lifeComponent = GetComponent<LifeComponent>();
            return lifeComponent != null && lifeComponent.Lives <= 0;
        }
    }
}
