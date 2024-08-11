using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using System;
using Kong_Engine.ECS.System;

namespace Kong_Engine.Objects
{
    public class PlayerSprite : BaseEntity
    {
        public Vector2 Knockback { get; set; }
        private Texture2D spriteSheet;
        private Rectangle[] walkFrames;
        private Rectangle[] idleFrames;
        private Rectangle[] jumpFrames;
        private float moveSpeed = 1.5f;
        private float jumpSpeed = 200f;
        private float gravity = 40f;
        private float fallMultiplier = 2.5f;  // Multiplier for gravity during descent
        private float horizontalAcceleration = 10f; // Acceleration rate for horizontal movement
        private float maxHorizontalSpeed = 150f; // Max speed for horizontal movement
        private float verticalAcceleration = 5f;  // Acceleration rate for gravity
        private Rectangle playerBounds; // For collisions
        private bool isIdle = true;
        private bool isMoving = false;
        private bool isJumping = false;
        private bool isFacingRight = true; // Flag to check direction
        private int currentFrame;
        private double frameTime;
        private double idleFrameTime; // Frame time for idle animation
        private double jumpFrameTime; // Frame time for jump animation
        private double timeSinceLastFrame;
        private int frameWidth = 30; // Width of each frame
        private int frameHeight = 37; // Height of each frame
        private float verticalSpeed = 0f; // Speed for jumping
        private TileMapManager tileMapManager;
        private float scale;

        public PlayerSprite(Texture2D spriteSheet, TileMapManager tileMapManager, float scale)
        {
            this.spriteSheet = spriteSheet;
            this.tileMapManager = tileMapManager;
            this.scale = scale;

            // Define the source rectangles for each frame
            idleFrames = new Rectangle[]
            {
                new Rectangle(0, 0, frameWidth, frameHeight),   // Idle Frame 1
                new Rectangle(24, 0, frameWidth, frameHeight),  // Idle Frame 2
                new Rectangle(47, 0, frameWidth, frameHeight),  // Idle Frame 3
            };

            walkFrames = new Rectangle[]
            {
                new Rectangle(71, 0, frameWidth, frameHeight),   // Walk Frame 1
                new Rectangle(94, 0, frameWidth, frameHeight),  // Walk Frame 2
                new Rectangle(119, 0, frameWidth, frameHeight),  // Walk Frame 3
                new Rectangle(143, 0, frameWidth, frameHeight),  // Walk Frame 4
                new Rectangle(166, 0, frameWidth, frameHeight), // Walk Frame 5
                new Rectangle(191, 0, frameWidth, frameHeight)  // Walk Frame 6
            };

            jumpFrames = new Rectangle[]
            {
                new Rectangle(0, 0, frameWidth, frameHeight),  // Jump Frame 1
                new Rectangle(263, 0, frameWidth, frameHeight), // Jump Frame 2
                new Rectangle(288, 0, frameWidth, frameHeight), // Jump Frame 3
            };

            // Initialize components with position and bounding box scaled correctly
            AddComponent(new PositionComponent { Position = new Vector2(100, 100) * scale });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, (int)(frameWidth * scale), (int)(frameHeight * scale))
            });
            AddComponent(new LifeComponent { Lives = 10 });
            AddComponent(new PhysicsComponent
            {
                Velocity = Vector2.Zero,
                Mass = 1f,
                IsGrounded = false
            });
            Knockback = Vector2.Zero;

            // Initialize player bounds with the correct scale
            UpdatePlayerBounds();

            currentFrame = 0;
            frameTime = 0.1; // Change frame every 0.1 seconds for walking animation
            idleFrameTime = 0.5; // Change frame every 0.5 seconds for idle animation
            jumpFrameTime = 0.05; // Change frame every 0.05 seconds for jump animation
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            ApplyKnockback();
            HandleInput(gameTime);

            var physicsComponent = GetComponent<PhysicsComponent>();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply gravity continuously
            var velocity = physicsComponent.Velocity;
            velocity.Y += gravity * deltaTime;
            physicsComponent.Velocity = velocity;
            AccelerationSystem.ApplyAcceleration(physicsComponent, deltaTime, gravity, fallMultiplier, verticalAcceleration, horizontalAcceleration, maxHorizontalSpeed, isMoving, isFacingRight);

            // Update position based on the current velocity
            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition.Y += physicsComponent.Velocity.Y * scale * deltaTime;
            GetComponent<PositionComponent>().Position = currentPosition;

            // Check for collision with the ground
            if (CheckCollisions(currentPosition))
            {
                // If the player hits the ground, stop vertical movement and reset jump state
                physicsComponent.IsGrounded = true;
                physicsComponent.Velocity = Vector2.Zero;  // Stop the bounce by setting velocity to zero

                isJumping = false;  // Stop the jump animation
            }
            else
            {
                // If no collision, the player is in the air
                physicsComponent.IsGrounded = false;
            }

            UpdateAnimationFrame(gameTime);
            UpdatePlayerBounds();

            // Update collision component bounding box
            var collisionComponent = GetComponent<CollisionComponent>();
            collisionComponent.BoundingBox = playerBounds;
        }






        private void HandleJumping()
        {
            var physicsComponent = GetComponent<PhysicsComponent>();

            if (physicsComponent.IsGrounded)
            {
                Console.WriteLine("Player is jumping!"); // Debugging line
                                                         // Apply upward velocity for the jump
                var velocity = physicsComponent.Velocity;
                velocity.Y = -jumpSpeed;  // Negative because Y increases downwards
                physicsComponent.Velocity = velocity;

                physicsComponent.IsGrounded = false;  // The player is now airborne
                isJumping = true;  // Set the flag to indicate jumping
            }
        }




        private void UpdateAnimationFrame(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= (isJumping ? jumpFrameTime : isMoving ? frameTime : idleFrameTime))
            {
                if (isJumping)
                {
                    currentFrame = (currentFrame + 1) % jumpFrames.Length;
                }
                else if (isMoving)
                {
                    currentFrame = (currentFrame + 1) % walkFrames.Length;
                }
                else
                {
                    currentFrame = (currentFrame + 1) % idleFrames.Length; // Cycle through idle frames
                }
                timeSinceLastFrame = 0;
            }
        }


        private void UpdatePlayerBounds()
        {
            var position = GetComponent<PositionComponent>().Position;
            playerBounds = new Rectangle(
                (int)(position.X - 8 * scale),
                (int)(position.Y - 8 * scale),
                (int)(frameWidth * scale),
                (int)(frameHeight * scale)
            );
        }

        public Rectangle GetBoundingBox()
        {
            var collisionComponent = GetComponent<CollisionComponent>();
            return collisionComponent?.BoundingBox ?? Rectangle.Empty;
        }

        private void ApplyKnockback()
        {
            if (Knockback != Vector2.Zero && isMoving)
            {
                var currentPosition = GetComponent<PositionComponent>().Position;
                currentPosition += Knockback * scale;
                Knockback *= 0.9f; // Decay the knockback over time

                if (Knockback.LengthSquared() < 0.01f)
                {
                    Knockback = Vector2.Zero;
                }

                GetComponent<PositionComponent>().Position = currentPosition;
            }
        }

        public void HandleMovement(Vector2 movement)
        {
            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition += movement * scale;

            // Check for collisions with the tilemap
            if (!CheckCollisions(currentPosition))
            {
                GetComponent<PositionComponent>().Position = currentPosition;
            }
            else
            {
                // Optional: Handle the collision response here
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            isIdle = true;
            isMoving = false;

            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= moveSpeed;
                isIdle = false;
                isMoving = true;
                isFacingRight = false; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += moveSpeed;
                isIdle = false;
                isMoving = true;
                isFacingRight = true; // Update direction flag
            }
            if (keyboardState.IsKeyDown(Keys.Space) && !isJumping)
            {
                HandleJumping();  // Call the jumping method
            }

            if (isMoving && !isJumping)
            {
                HandleMovement(movement);
            }
        }


        private bool CheckCollisions(Vector2 newPosition)
        {
            var newBounds = new Rectangle(
                (int)(newPosition.X - 8 * scale),
                (int)(newPosition.Y - 8 * scale),
                playerBounds.Width,
                playerBounds.Height
            );

            foreach (var rect in tileMapManager.CollisionRectangles)
            {
                if (newBounds.Intersects(rect))
                {
                    Console.WriteLine($"Collision detected at ({rect.X}, {rect.Y})");

                    // If the collision is below the player, consider them grounded
                    var physicsComponent = GetComponent<PhysicsComponent>();
                    if (newBounds.Bottom >= rect.Top && newBounds.Top < rect.Top)
                    {
                        physicsComponent.IsGrounded = true;
                        Console.WriteLine("Player is grounded!"); // Debugging line
                    }
                    return true; // Collision detected
                }
            }

            // If no collision, player is in the air
            var pc = GetComponent<PhysicsComponent>();
            pc.IsGrounded = false;
            Console.WriteLine("Player is in the air!"); // Debugging line

            return false; // No collision
        }



        public void Move(Vector2 direction)
        {
            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition += direction * scale;
            GetComponent<PositionComponent>().Position = currentPosition;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle currentFrameRect;
            if (isJumping)
            {
                currentFrameRect = jumpFrames[currentFrame % jumpFrames.Length];
            }
            else if (isMoving)
            {
                currentFrameRect = walkFrames[currentFrame % walkFrames.Length];
            }
            else
            {
                currentFrameRect = idleFrames[currentFrame % idleFrames.Length];
            }

            var position = GetComponent<PositionComponent>().Position;

            // Flip the sprite if facing left
            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }
    }
}
