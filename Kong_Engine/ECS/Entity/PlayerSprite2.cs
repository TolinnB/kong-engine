using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;

namespace Kong_Engine.Objects
{
    public class PlayerSprite2 : BaseEntity
    {
        public Vector2 Knockback { get; set; }
        private Texture2D spriteSheet;
        private Rectangle defaultFrame;
        private Rectangle leftTurnFrame;
        private Rectangle rightTurnFrame;
        private Rectangle defaultTurboFrame; // Turbo frames
        private Rectangle leftTurnTurboFrame;
        private Rectangle rightTurnTurboFrame;
        private Rectangle missileFrame1;
        private Rectangle missileFrame2;
        private float moveSpeed = 1.5f;
        private float turboMultiplier = 2.0f; // Multiplier for turbo speed
        private Rectangle playerBounds; // For collisions
        private bool isFacingRight = true; // Flag to check direction
        private bool isTurboActive = false; // Flag to check turbo state
        private float scale;
        private int frameWidth = 42; // Width of each frame
        private int frameHeight = 43; // Height of each frame

        private bool isHit = false;
        private double hitTimer = 0;

        private List<Missile> missiles;
        private float missileSpeed = 300f;
        private float missileAnimationTime = 0.1f; // Time between frame changes for the missile
        private float missileCooldown = 0.5f; // Cooldown time in seconds
        private double lastMissileFireTime = 0; // Last time a missile was fired
        public int Score { get; private set; } // Score property

        public PlayerSprite2(Texture2D spriteSheet, float scale)
        {
            this.spriteSheet = spriteSheet;
            this.scale = scale;

            missiles = new List<Missile>();

            // Define the source rectangles for each frame
            defaultFrame = new Rectangle(40, 0, frameWidth, frameHeight);   // Default flying frame
            leftTurnFrame = new Rectangle(0, 0, frameWidth, frameHeight);  // Turning left frame
            rightTurnFrame = new Rectangle(82, 0, frameWidth, frameHeight); // Turning right frame

            // Define turbo frames
            defaultTurboFrame = new Rectangle(40, 44, frameWidth, frameHeight);    // Turbo default frame
            leftTurnTurboFrame = new Rectangle(0, 44, frameWidth, frameHeight);  // Turbo turning left frame
            rightTurnTurboFrame = new Rectangle(82, 44, frameWidth, frameHeight); // Turbo turning right frame

            // Define missile frames
            missileFrame1 = new Rectangle(56, 130, 10, 20); // Example frame 1 for the missile
            missileFrame2 = new Rectangle(64, 130, 10, 20); // Example frame 2 for the missile

            AddComponent(new PositionComponent { Position = new Vector2(650, 625) }); // Start position set here
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, (int)(frameWidth * scale), (int)(frameHeight * scale))
            });
            AddComponent(new LifeComponent { Lives = 3 });
            Knockback = Vector2.Zero;

            playerBounds = new Rectangle((int)GetComponent<PositionComponent>().Position.X - 8, (int)GetComponent<PositionComponent>().Position.Y - 8, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public void Update(GameTime gameTime, List<Asteroid> asteroids)
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

            // Update missiles
            for (int i = missiles.Count - 1; i >= 0; i--)
            {
                missiles[i].Update(gameTime);
                if (missiles[i].GetComponent<PositionComponent>().Position.Y < 0)
                {
                    missiles.RemoveAt(i);
                }
                else
                {
                    // Check for collisions with asteroids
                    foreach (var asteroid in asteroids)
                    {
                        if (missiles[i].GetBoundingBox().Intersects(asteroid.GetBoundingBox()))
                        {
                            missiles.RemoveAt(i);
                            asteroids.Remove(asteroid);
                            Score += 10; // Increase score by 10 for each asteroid destroyed
                            break;
                        }
                    }
                }
            }

            // Log bounding box position for debugging
            Debug.WriteLine($"Player bounding box: {playerBounds}");

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
                Debug.WriteLine($"Player hit! Lives left: {lifeComponent.Lives}");
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
            if (keyboardState.IsKeyDown(Keys.J))
            {
                movement.X -= moveSpeed;
                isFacingRight = false; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.L))
            {
                movement.X += moveSpeed;
                isFacingRight = true; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.I))
            {
                movement.Y -= moveSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.K))
            {
                movement.Y += moveSpeed;
            }

            // Check for turbo activation
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                isTurboActive = true;
            }
            else
            {
                isTurboActive = false;
            }

            // Fire missile
            if (keyboardState.IsKeyDown(Keys.Z) && gameTime.TotalGameTime.TotalSeconds > lastMissileFireTime + missileCooldown)
            {
                FireMissile();
                lastMissileFireTime = gameTime.TotalGameTime.TotalSeconds;
            }

            var currentPosition = GetComponent<PositionComponent>().Position;
            float currentMoveSpeed = isTurboActive ? moveSpeed * turboMultiplier : moveSpeed;
            currentPosition += movement * currentMoveSpeed;

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

        private void FireMissile()
        {
            var positionComponent = GetComponent<PositionComponent>();
            Vector2 missilePosition = new Vector2(positionComponent.Position.X + (frameWidth * scale) / 2 - missileFrame1.Width / 2, positionComponent.Position.Y);
            missiles.Add(new Missile(spriteSheet, missileFrame1, missileFrame2, missilePosition, missileSpeed, missileAnimationTime));
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle currentFrameRect;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.J))
            {
                currentFrameRect = isTurboActive ? leftTurnTurboFrame : leftTurnFrame;
            }
            else if (keyboardState.IsKeyDown(Keys.L))
            {
                currentFrameRect = isTurboActive ? rightTurnTurboFrame : rightTurnFrame;
            }
            else
            {
                currentFrameRect = isTurboActive ? defaultTurboFrame : defaultFrame;
            }

            var position = GetComponent<PositionComponent>().Position;

            // Flip the sprite if facing left
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Flash white when hit
            Color drawColor = isHit ? Color.Red : Color.White;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, drawColor, 0f, Vector2.Zero, scale, spriteEffects, 0f);

            // Draw missiles
            foreach (var missile in missiles)
            {
                missile.Draw(spriteBatch);
            }
        }
    }
}
