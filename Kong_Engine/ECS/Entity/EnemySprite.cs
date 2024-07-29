using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class EnemySprite : BaseEntity
    {
        private Texture2D spriteSheet;
        private Rectangle[] walkFrames;
        private int currentFrame;
        private double frameTime;
        private double timeSinceLastFrame;
        private bool isMovingRight = true;
        private int frameWidth = 49; // Width of each frame
        private int frameHeight = 50; // Height of each frame

        public EnemySprite(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;

            // Define the source rectangles for each frame
            walkFrames = new Rectangle[]
            {
                new Rectangle(-5, 45, frameWidth, frameHeight),   // Walk Frame 1
                new Rectangle(42, 45, frameWidth, frameHeight),  // Walk Frame 2
                new Rectangle(89, 45, frameWidth, frameHeight), // Walk Frame 3
            };

            AddComponent(new PositionComponent { Position = new Vector2(1000, 100) });
            AddComponent(new MovementComponent
            {
                Velocity = new Vector2(2, 0),
                LeftBoundary = 800,
                RightBoundary = 1200,
                MovingRight = true
            });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(1000, 100, frameWidth, frameHeight)
            });

            currentFrame = 0;
            frameTime = 0.1; // Change frame every 0.1 seconds
            timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= frameTime)
            {
                currentFrame = (currentFrame + 1) % walkFrames.Length;
                timeSinceLastFrame = 0;
            }

            var movementComponent = GetComponent<MovementComponent>();
            var positionComponent = GetComponent<PositionComponent>();

            // Move the enemy
            if (movementComponent.MovingRight)
            {
                positionComponent.Position += movementComponent.Velocity;
                if (positionComponent.Position.X >= movementComponent.RightBoundary)
                {
                    movementComponent.MovingRight = false;
                }
            }
            else
            {
                positionComponent.Position -= movementComponent.Velocity;
                if (positionComponent.Position.X <= movementComponent.LeftBoundary)
                {
                    movementComponent.MovingRight = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: matrix);

            Rectangle currentFrameRect = walkFrames[currentFrame];
            var position = GetComponent<PositionComponent>().Position;

            // Flip the sprite if moving left
            SpriteEffects spriteEffects = GetComponent<MovementComponent>().MovingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);

            spriteBatch.End();
        }
    }
}
