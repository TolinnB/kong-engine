using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;

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
        private float jumpSpeed = 10f;
        private float gravity = 0.5f;
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

        public PlayerSprite(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;

            // Define the source rectangles for each frame
            idleFrames = new Rectangle[]
            {
                new Rectangle(0, 0, frameWidth, frameHeight),   // Idle Frame 1
                new Rectangle(32, 0, frameWidth, frameHeight),  // Idle Frame 2
                new Rectangle(64, 0, frameWidth, frameHeight),  // Idle Frame 3
                new Rectangle(96, 0, frameWidth, frameHeight)   // Idle Frame 4
            };

            walkFrames = new Rectangle[]
            {
                new Rectangle(5, 40, frameWidth, frameHeight),   // Walk Frame 1
                new Rectangle(38, 40, frameWidth, frameHeight),  // Walk Frame 2
                new Rectangle(74, 40, frameWidth, frameHeight),  // Walk Frame 3
                new Rectangle(109, 40, frameWidth, frameHeight),  // Walk Frame 4
                new Rectangle(139, 40, frameWidth, frameHeight), // Walk Frame 5
                new Rectangle(175, 40, frameWidth, frameHeight)  // Walk Frame 6
            };

            jumpFrames = new Rectangle[]
            {
                new Rectangle(0, 116, frameWidth, frameHeight),  // Jump Frame 1
                new Rectangle(37, 116, frameWidth, frameHeight), // Jump Frame 2
                new Rectangle(73, 116, frameWidth, frameHeight), // Jump Frame 3
                new Rectangle(104, 115, frameWidth, frameHeight),  // Jump Frame 4
                new Rectangle(140, 115, frameWidth, frameHeight)  // Jump Frame 5
            };

            AddComponent(new PositionComponent { Position = new Vector2(100, 100) }); // Start position set here
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, frameWidth, frameHeight)
            });
            AddComponent(new LifeComponent { Lives = 3 });
            Knockback = Vector2.Zero;

            playerBounds = new Rectangle((int)Position.X - 8, (int)Position.Y - 8, frameWidth, frameHeight);

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

            if (isJumping)
            {
                verticalSpeed -= gravity;
                var currentPosition = GetComponent<PositionComponent>().Position;
                currentPosition.Y -= verticalSpeed;

                // Check if player has landed
                if (currentPosition.Y >= 100) // Assuming ground level is y=100
                {
                    currentPosition.Y = 100;
                    isJumping = false;
                    verticalSpeed = 0f;
                }

                GetComponent<PositionComponent>().Position = currentPosition;
            }

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

            var position = GetComponent<PositionComponent>().Position;
            playerBounds.X = (int)position.X - 8;
            playerBounds.Y = (int)position.Y - 8;
        }

        private void ApplyKnockback()
        {
            if (Knockback != Vector2.Zero && isMoving)
            {
                var currentPosition = GetComponent<PositionComponent>().Position;
                currentPosition += Knockback;
                Knockback *= 0.9f; // Decay the knockback over time

                if (Knockback.LengthSquared() < 0.01f)
                {
                    Knockback = Vector2.Zero;
                }

                GetComponent<PositionComponent>().Position = currentPosition;
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
                isJumping = true;
                verticalSpeed = jumpSpeed; // Initiate jump
            }

            if (isMoving && !isJumping)
            {
                var currentPosition = GetComponent<PositionComponent>().Position;
                currentPosition += movement;
                GetComponent<PositionComponent>().Position = currentPosition;
            }
        }

        public void Move(Vector2 direction)
        {
            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition += direction;
            GetComponent<PositionComponent>().Position = currentPosition;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: matrix);

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

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);

            spriteBatch.End();
        }
    }
}
