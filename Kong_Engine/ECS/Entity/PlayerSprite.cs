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
        private float fallMultiplier = 2.5f;
        private float horizontalAcceleration = 50f;
        private float maxHorizontalSpeed = 150f;
        private float verticalAcceleration = 5f;
        private Rectangle playerBounds;
        private bool isIdle = true;
        private bool isMoving = false;
        private bool isJumping = false;
        private bool isFacingRight = true;
        private int currentFrame;
        private double frameTime;
        private double idleFrameTime;
        private double jumpFrameTime;
        private double timeSinceLastFrame;
        private int frameWidth = 28;
        private int frameHeight = 30;
        private float verticalSpeed = 0f;
        private TileMapManager tileMapManager;
        private float scale;

        private const int mapWidth = 1280; // Screen width
        private const int mapHeight = 720; // Screen height

        public PlayerSprite(Texture2D spriteSheet, TileMapManager tileMapManager, float scale)
        {
            this.spriteSheet = spriteSheet;
            this.tileMapManager = tileMapManager;
            this.scale = scale;

            idleFrames = new Rectangle[]
            {
                new Rectangle(0, 0, frameWidth, frameHeight),
                new Rectangle(24, 0, frameWidth, frameHeight),
                new Rectangle(47, 0, frameWidth, frameHeight),
            };

            walkFrames = new Rectangle[]
            {
                new Rectangle(71, 0, frameWidth, frameHeight),
                new Rectangle(94, 0, frameWidth, frameHeight),
                new Rectangle(119, 0, frameWidth, frameHeight),
                new Rectangle(143, 0, frameWidth, frameHeight),
                new Rectangle(166, 0, frameWidth, frameHeight),
                new Rectangle(191, 0, frameWidth, frameHeight)
            };

            jumpFrames = new Rectangle[]
            {
                new Rectangle(288, 0, frameWidth, frameHeight),
            };

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

            UpdatePlayerBounds();

            currentFrame = 0;
            frameTime = 0.1;
            idleFrameTime = 0.5;
            jumpFrameTime = 0.05;
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            ApplyKnockback();
            HandleInput(gameTime);

            var physicsComponent = GetComponent<PhysicsComponent>();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var velocity = physicsComponent.Velocity;
            velocity.Y += gravity * deltaTime;
            physicsComponent.Velocity = velocity;
            AccelerationSystem.ApplyAcceleration(physicsComponent, deltaTime, gravity, fallMultiplier, verticalAcceleration, horizontalAcceleration, maxHorizontalSpeed, isMoving, isFacingRight);

            var currentPosition = GetComponent<PositionComponent>().Position;
            currentPosition.X += physicsComponent.Velocity.X * scale * deltaTime;
            currentPosition.Y += physicsComponent.Velocity.Y * scale * deltaTime;

            currentPosition.X = MathHelper.Clamp(currentPosition.X, 0, mapWidth - playerBounds.Width);

            GetComponent<PositionComponent>().Position = currentPosition;

            if (CheckCollisions(currentPosition))
            {
                physicsComponent.IsGrounded = true;
                physicsComponent.Velocity = Vector2.Zero;
                isJumping = false;
            }
            else
            {
                physicsComponent.IsGrounded = false;
            }

            UpdateAnimationFrame(gameTime);
            UpdatePlayerBounds();

            var collisionComponent = GetComponent<CollisionComponent>();
            collisionComponent.BoundingBox = playerBounds;
        }

        private void HandleJumping()
        {
            var physicsComponent = GetComponent<PhysicsComponent>();

            if (physicsComponent.IsGrounded)
            {
                var velocity = physicsComponent.Velocity;
                velocity.Y = -jumpSpeed;
                physicsComponent.Velocity = velocity;

                physicsComponent.IsGrounded = false;
                isJumping = true;
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
                    currentFrame = (currentFrame + 1) % idleFrames.Length;
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
                Knockback *= 0.9f;

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

            if (!CheckCollisions(currentPosition))
            {
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
                isFacingRight = false;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += moveSpeed;
                isIdle = false;
                isMoving = true;
                isFacingRight = true;
            }
            if (keyboardState.IsKeyDown(Keys.Space) && !isJumping)
            {
                HandleJumping();
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
                    var physicsComponent = GetComponent<PhysicsComponent>();
                    if (newBounds.Bottom >= rect.Top && newBounds.Top < rect.Top)
                    {
                        physicsComponent.IsGrounded = true;
                    }
                    return true;
                }
            }

            var pc = GetComponent<PhysicsComponent>();
            pc.IsGrounded = false;

            return false;
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

            SpriteEffects spriteEffects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }
    }
}
