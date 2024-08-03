using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;

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
        private Body playerBody; // Physics body

        public PlayerSprite(Texture2D spriteSheet, World world)
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

            // Create player physics body
            playerBody = world.CreateRectangle(
                ConvertUnits.ToSimUnits(frameWidth),
                ConvertUnits.ToSimUnits(frameHeight),
                1f,
                ConvertUnits.ToSimUnits(new Vector2(100, 100))
            );
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.FixedRotation = true;
            playerBody.Tag = "player"; // Use Tag property instead of UserData
        }

        public Body PlayerBody => playerBody; // Public property to access playerBody
    }
}