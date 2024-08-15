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
        private int frameWidth = 49; // Width of each frame
        private int frameHeight = 50; // Height of each frame
        private float scale; // Scale factor

        public EnemySprite(Texture2D spriteSheet, float scale)
        {
            this.spriteSheet = spriteSheet;
            this.scale = scale;

            walkFrames = new Rectangle[]
            {
        new Rectangle(0, 0, frameWidth, frameHeight),   // Walk Frame 1
        new Rectangle(135, 0, frameWidth, frameHeight),  // Walk Frame 2
        new Rectangle(270, 0, frameWidth, frameHeight), // Walk Frame 3
        new Rectangle(405, 0, frameWidth, frameHeight), // Walk Frame 4
        new Rectangle(530, 0, frameWidth, frameHeight), // Walk Frame 5
            };

            AddComponent(new PositionComponent { Position = new Vector2(310, 213) * scale });
            AddComponent(new MovementComponent
            {
                Velocity = new Vector2(2, 0) * scale,
                LeftBoundary = 310 * scale, // Adjusted boundaries
                RightBoundary = 390 * scale, // Adjusted boundaries
                MovingRight = true
            });

            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle((int)(200 * scale), (int)(300 * scale), (int)(frameWidth * scale), (int)(frameHeight * scale))
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

            var positionComponent = GetComponent<PositionComponent>();
            var collisionComponent = GetComponent<CollisionComponent>();
            collisionComponent.BoundingBox = new Rectangle(
                (int)positionComponent.Position.X,
                (int)positionComponent.Position.Y,
                (int)(frameWidth * scale),
                (int)(frameHeight * scale)
            );
        }

        public void Draw(SpriteBatch spriteBatch, Matrix matrix)
        {
            Rectangle currentFrameRect = walkFrames[currentFrame];
            var position = GetComponent<PositionComponent>().Position;

            SpriteEffects spriteEffects = GetComponent<MovementComponent>().MovingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(spriteSheet, position, currentFrameRect, Color.White, 0f, Vector2.Zero, scale, spriteEffects, 0f);
        }
    }
}