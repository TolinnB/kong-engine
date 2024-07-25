using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.Objects
{
    public class PlayerSprite : BaseEntity
    {
        public Vector2 Knockback { get; set; }
        private Texture2D[] idleTextures;
        private Texture2D[] walkTextures;
        private float moveSpeed = 1.5f;
        private Rectangle playerBounds; // For collisions
        private bool isIdle = false;
        private int currentFrame;
        private double frameTime;
        private double timeSinceLastFrame;

        public PlayerSprite(Texture2D[] idleTextures, Texture2D[] walkTextures)
        {
            this.idleTextures = idleTextures;
            this.walkTextures = walkTextures;

            AddComponent(new PositionComponent { Position = Vector2.Zero });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(0, 0, idleTextures[0].Width, idleTextures[0].Height)
            });
            AddComponent(new LifeComponent { Lives = 10 });
            Knockback = Vector2.Zero;

            playerBounds = new Rectangle((int)Position.X - 8, (int)Position.Y - 8, idleTextures[0].Width, idleTextures[0].Height);

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
                currentFrame = (currentFrame + 1) % (isIdle ? idleTextures.Length : walkTextures.Length);
                timeSinceLastFrame = 0;
            }

            playerBounds.X = (int)Position.X - 8;
            playerBounds.Y = (int)Position.Y - 8;
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
            if (keyboardState.IsKeyDown(Keys.A))
            {
                movement.X -= moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                movement.X += moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                movement.Y -= moveSpeed;
                isIdle = false;
            }
            if (keyboardState.IsKeyDown(Keys.S))
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

            Texture2D currentTexture = isIdle ? idleTextures[currentFrame] : walkTextures[currentFrame];
            spriteBatch.Draw(currentTexture, Position, Color.White);

            spriteBatch.End();
        }
    }
}
