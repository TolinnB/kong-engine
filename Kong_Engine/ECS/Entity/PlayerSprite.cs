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
        private Rectangle[] idleFrames;
        private Rectangle[] walkFrames;
        private float moveSpeed = 1.5f;
        private Rectangle playerBounds; // For collisions
        private bool isIdle = true;
        private int currentFrame;
        private double frameTime;
        private double timeSinceLastFrame;
        private int frameWidth = 16; // Width of each frame
        private int frameHeight = 32; // Height of each frame

        public PlayerSprite(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;

            // Define the source rectangles for each frame
            idleFrames = new Rectangle[]
            {
                new Rectangle(0, 0, frameWidth, frameHeight),
                new Rectangle(frameWidth, 0, frameWidth, frameHeight),
                new Rectangle(2 * frameWidth, 0, frameWidth, frameHeight),
                new Rectangle(3 * frameWidth, 0, frameWidth, frameHeight)
            };

            walkFrames = new Rectangle[]
            {
                new Rectangle(0, frameHeight, frameWidth, frameHeight),
                new Rectangle(frameWidth, frameHeight, frameWidth, frameHeight),
                new Rectangle(2 * frameWidth, frameHeight, frameWidth, frameHeight),
                new Rectangle(3 * frameWidth, frameHeight, frameWidth, frameHeight)
            };

            AddComponent(new PositionComponent { Position = new Vector2(100, 100) }); // Start position set here
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, frameWidth, frameHeight)
            });
            AddComponent(new LifeComponent { Lives = 10 });
            Knockback = Vector2.Zero;

            playerBounds = new Rectangle((int)Position.X - 8, (int)Position.Y - 8, frameWidth, frameHeight);

            currentFrame = 0;
            frameTime = 0.1; // Change frame every 0.1 seconds
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            ApplyKnockback();
            HandleInput(gameTime);
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= frameTime)
            {
                currentFrame = (currentFrame + 1) % (isIdle ? idleFrames.Length : walkFrames.Length);
                timeSinceLastFrame = 0;
            }

            var position = GetComponent<PositionComponent>().Position;
            playerBounds.X = (int)position.X - 8;
            playerBounds.Y = (int)position.Y - 8;
        }

        private void ApplyKnockback()
        {
            if (Knockback != Vector2.Zero)
            {
                var position = GetComponent<PositionComponent>();
                position.Position += Knockback;
                Knockback *= 0.9f; // Decay the knockback over time

                if (Knockback.LengthSquared() < 0.01f)
                {
                    Knockback = Vector2.Zero;
                }
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            isIdle = true;

            Vector2 movement = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                movement.X -= moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                movement.X += moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                movement.Y -= moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                movement.Y += moveSpeed;
                isIdle = false;
            }

            var position = GetComponent<PositionComponent>();
            position.Position += movement;
        }

        public void Move(Vector2 direction)
        {
            var position = GetComponent<PositionComponent>();
            position.Position += direction;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: matrix);

            Rectangle currentFrameRect = isIdle ? idleFrames[currentFrame] : walkFrames[currentFrame];
            var position = GetComponent<PositionComponent>().Position;
            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White);

            spriteBatch.End();
        }
    }
}
