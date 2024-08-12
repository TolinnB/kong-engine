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
        private Rectangle[] downIdleFrames;
        private Rectangle[] leftIdleFrames;
        private Rectangle[] rightIdleFrames;
        private Rectangle[] upIdleFrames;
        private Rectangle[] downMoveFrames;
        private Rectangle[] leftMoveFrames;
        private Rectangle[] rightMoveFrames;
        private Rectangle[] upMoveFrames;
        private float moveSpeed = 1.5f;
        private Rectangle playerBounds; // For collisions
        private Direction currentDirection;
        private float scale;  // Increased scale factor
        private int frameWidth = 20; // Width of each frame
        private int frameHeight = 20; // Height of each frame
        private int currentFrame;
        private double frameTimer;
        private bool isMoving; // New variable to track movement state

        // Parameters for fine-tuning each idle animation
        private double downFrameInterval = 0.2;
        private double leftFrameInterval = 0.2;
        private double rightFrameInterval = 0.2;
        private double upFrameInterval = 0.2;

        private bool isHit = false;
        private double hitTimer = 0;
        public int Score { get; private set; } // Score property
        private readonly AudioManager _audioManager;

        public enum Direction
        {
            Down,
            Left,
            Right,
            Up
        }

        public RaccoonSprite(Texture2D spriteSheet, float scale, AudioManager audioManager)
        {
            this.spriteSheet = spriteSheet;
            this.scale = scale * 20f;  // Increase the scale factor to make the sprite larger
            this._audioManager = audioManager;

            // Define the source rectangles for each idle frame
            downIdleFrames = new Rectangle[]
            {
                new Rectangle(6, 9, frameWidth, frameHeight),
                new Rectangle(38, 8, frameWidth, frameHeight),
                new Rectangle(70, 8, frameWidth, frameHeight),
                new Rectangle(102, 7, frameWidth, frameHeight),
                new Rectangle(6, 9, frameWidth, frameHeight),
                new Rectangle(38, 8, frameWidth, frameHeight),
                new Rectangle(70, 8, frameWidth, frameHeight),
                new Rectangle(102, 7, frameWidth, frameHeight)
            };

            leftIdleFrames = new Rectangle[]
            {
                new Rectangle(6, 70, frameWidth, frameHeight),
                new Rectangle(38, 70, frameWidth, frameHeight),
                new Rectangle(70, 70, frameWidth, frameHeight),
                new Rectangle(102, 70, frameWidth, frameHeight),
                new Rectangle(6, 70, frameWidth, frameHeight),
                new Rectangle(38, 70, frameWidth, frameHeight),
                new Rectangle(70, 70, frameWidth, frameHeight),
                new Rectangle(102, 70, frameWidth, frameHeight)
            };

            rightIdleFrames = new Rectangle[]
            {
                new Rectangle(4, 37, frameWidth, frameHeight),
                new Rectangle(36, 37, frameWidth, frameHeight),
                new Rectangle(68, 37, frameWidth, frameHeight),
                new Rectangle(100, 37, frameWidth, frameHeight),
                new Rectangle(4, 37, frameWidth, frameHeight),
                new Rectangle(36, 37, frameWidth, frameHeight),
                new Rectangle(68, 37, frameWidth, frameHeight),
                new Rectangle(100, 37, frameWidth, frameHeight)
            };

            upIdleFrames = new Rectangle[]
            {
                new Rectangle(6, 102, frameWidth, frameHeight),
                new Rectangle(38, 102, frameWidth, frameHeight),
                new Rectangle(70, 102, frameWidth, frameHeight),
                new Rectangle(100, 102, frameWidth, frameHeight),
                new Rectangle(6, 102, frameWidth, frameHeight),
                new Rectangle(38, 102, frameWidth, frameHeight),
                new Rectangle(70, 102, frameWidth, frameHeight),
                new Rectangle(100, 102, frameWidth, frameHeight)

            };  

            // Define the source rectangles for each movement frame
            downMoveFrames = new Rectangle[]
            {
                new Rectangle(6, 163, frameWidth, frameHeight),
                new Rectangle(38, 163, frameWidth, frameHeight),
                new Rectangle(70, 163, frameWidth, frameHeight),
                new Rectangle(102, 163, frameWidth, frameHeight),
                new Rectangle(4, 194, frameWidth, frameHeight),
                new Rectangle(38, 194, frameWidth, frameHeight),
                new Rectangle(70, 194, frameWidth, frameHeight),
                new Rectangle(103, 194, frameWidth, frameHeight)
            };

            leftMoveFrames = new Rectangle[]
            {
                new Rectangle(8, 227, frameWidth, frameHeight),
                new Rectangle(40, 227, frameWidth, frameHeight),
                new Rectangle(72, 227, frameWidth, frameHeight),
                new Rectangle(104, 227, frameWidth, frameHeight),
                new Rectangle(8, 262, frameWidth, frameHeight),
                new Rectangle(40, 262, frameWidth, frameHeight),
                new Rectangle(72, 262, frameWidth, frameHeight),
                new Rectangle(104, 262, frameWidth, frameHeight)
            };

            rightMoveFrames = new Rectangle[]
            {
                new Rectangle(6, 294, frameWidth, frameHeight),
                new Rectangle(38, 294, frameWidth, frameHeight),
                new Rectangle(70, 294, frameWidth, frameHeight),
                new Rectangle(102, 294, frameWidth, frameHeight),
                new Rectangle(6, 326, frameWidth, frameHeight),
                new Rectangle(38, 326, frameWidth, frameHeight),
                new Rectangle(70, 326, frameWidth, frameHeight),
                new Rectangle(103, 326, frameWidth, frameHeight)
            };

            upMoveFrames = new Rectangle[]
            {
                new Rectangle(7, 357, frameWidth, frameHeight),
                new Rectangle(39, 357, frameWidth, frameHeight),
                new Rectangle(71, 357, frameWidth, frameHeight),
                new Rectangle(103, 357, frameWidth, frameHeight),
                new Rectangle(7, 389, frameWidth, frameHeight),
                new Rectangle(39, 389, frameWidth, frameHeight),
                new Rectangle(71, 389, frameWidth, frameHeight),
                new Rectangle(103, 389, frameWidth, frameHeight)
            };

            currentFrame = 0;
            frameTimer = 0;
            currentDirection = Direction.Down; // Default starting direction
            isMoving = false;

            AddComponent(new PositionComponent { Position = new Vector2(100, 100) }); // Start position set here
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

            // Update the animation frame based on the current direction and movement state
            if (isMoving)
            {
                UpdateMovementFrames(gameTime);
            }
            else
            {
                UpdateIdleFrames(gameTime);
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

        private void UpdateMovementFrames(GameTime gameTime)
        {
            double frameInterval = GetFrameIntervalForCurrentDirection(); // Use the same intervals
            int frameCount = GetCurrentFrameCountMovement(); // Get movement frame count

            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer > frameInterval)
            {
                frameTimer = 0;
                currentFrame = (currentFrame + 1) % frameCount; // Cycle through movement frames
            }
        }

        private void UpdateIdleFrames(GameTime gameTime)
        {
            double frameInterval = GetFrameIntervalForCurrentDirection();
            int frameCount = GetCurrentFrameCountIdle(); // Get idle frame count

            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer > frameInterval)
            {
                frameTimer = 0;
                currentFrame = (currentFrame + 1) % frameCount; // Cycle through idle frames
            }
        }

        private int GetCurrentFrameCountMovement()
        {
            return currentDirection switch
            {
                Direction.Down => downMoveFrames.Length,
                Direction.Left => leftMoveFrames.Length,
                Direction.Right => rightMoveFrames.Length,
                Direction.Up => upMoveFrames.Length,
                _ => downMoveFrames.Length,
            };
        }

        private int GetCurrentFrameCountIdle()
        {
            return currentDirection switch
            {
                Direction.Down => downIdleFrames.Length,
                Direction.Left => leftIdleFrames.Length,
                Direction.Right => rightIdleFrames.Length,
                Direction.Up => upIdleFrames.Length,
                _ => downIdleFrames.Length,
            };
        }

        private double GetFrameIntervalForCurrentDirection()
        {
            return currentDirection switch
            {
                Direction.Down => downFrameInterval,
                Direction.Left => leftFrameInterval,
                Direction.Right => rightFrameInterval,
                Direction.Up => upFrameInterval,
                _ => 0.2,
            };
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
            isMoving = false;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= moveSpeed;
                currentDirection = Direction.Left;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += moveSpeed;
                currentDirection = Direction.Right;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                movement.Y -= moveSpeed;
                currentDirection = Direction.Up;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y += moveSpeed;
                currentDirection = Direction.Down;
                isMoving = true;
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
            Rectangle currentFrameRect = currentDirection switch
            {
                Direction.Left => isMoving ? leftMoveFrames[currentFrame] : leftIdleFrames[currentFrame],
                Direction.Right => isMoving ? rightMoveFrames[currentFrame] : rightIdleFrames[currentFrame],
                Direction.Up => isMoving ? upMoveFrames[currentFrame] : upIdleFrames[currentFrame],
                Direction.Down => isMoving ? downMoveFrames[currentFrame] : downIdleFrames[currentFrame],
                _ => downIdleFrames[currentFrame],
            };

            var position = GetComponent<PositionComponent>().Position;

            // Flip the sprite if facing left
            SpriteEffects spriteEffects = currentDirection == Direction.Left ? SpriteEffects.None : SpriteEffects.None;

            // Flash red when hit
            Color drawColor = isHit ? Color.Red : Color.White;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, drawColor, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }

        public bool IsDead()
        {
            var lifeComponent = GetComponent<LifeComponent>();
            return lifeComponent != null && lifeComponent.Lives <= 0;
        }

        // Methods to set fine-tuning parameters
        public void SetDownFrameInterval(double interval) => downFrameInterval = interval;
        public void SetLeftFrameInterval(double interval) => leftFrameInterval = interval;
        public void SetRightFrameInterval(double interval) => rightFrameInterval = interval;
        public void SetUpFrameInterval(double interval) => upFrameInterval = interval;
    }
}
