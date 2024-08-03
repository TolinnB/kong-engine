using Kong_Engine.Enum;
using Kong_Engine.States;
using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
<<<<<<< HEAD
using TiledSharp;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
=======
>>>>>>> main

namespace Kong_Engine
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;
        private const int DesignedResolutionWidth = 1280;
        private const int DesignedResolutionHeight = 720;
        private const float DesignedResolutionAspectRatio = DesignedResolutionWidth / (float)DesignedResolutionHeight;
        private BaseGameState _currentState;
        private AudioManager _audioManager;
<<<<<<< HEAD
        private KeyboardState _previousKeyboardState;
        private InputManager _inputManager;
        private Texture2D _mainMenuBackground;
        private Texture2D _splashScreen;
        private Texture2D _endLevelSummaryBackground;
        private SpriteFont _gameOverFont;
        private Texture2D _BallSprite;

        // Tiled map variables
        private TileMapManager _tileMapManager;
        private List<Rectangle> collisionRectangles;

        // Physics variables
        private World physicsWorld;
        private Body playerBody;

        // PhysicsBallState variables
        private PhysicsBallState _physicsBallState;
=======
        private float _scaleFactor = 1f;
>>>>>>> main

        // New Physics Systems
        private GravitySystem _gravitySystem;
        private JumpingSystem _jumpingSystem;
        private BuoyancySystem _buoyancySystem;
        private FrictionAndDragSystem _frictionAndDragSystem;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = DesignedResolutionWidth;
            _graphics.PreferredBackBufferHeight = DesignedResolutionHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, DesignedResolutionWidth, DesignedResolutionHeight, false,
                SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            _renderScaleRectangle = GetScaleRectangle();
            _scaleFactor = (float)Window.ClientBounds.Width / DesignedResolutionWidth;

            base.Initialize();
<<<<<<< HEAD

            // Create a new physics world
            physicsWorld = new World(new Vector2(0, 9.8f)); // Gravity pointing down

            // Initialize physics systems
            _gravitySystem = new GravitySystem(new Vector2(0, 9.8f));
            _jumpingSystem = new JumpingSystem(10f, 9.8f);
            _buoyancySystem = new BuoyancySystem(200, 1f); // Example values
            _frictionAndDragSystem = new FrictionAndDragSystem(0.1f, 0.02f); // Example values
=======
            _audioManager = new AudioManager(Content);
            SwitchState(new SplashState());
>>>>>>> main
        }

        public float ScaleFactor => _scaleFactor;

        private Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            Rectangle scaleRectangle;

            if (actualAspectRatio <= DesignedResolutionAspectRatio)
            {
                var presentHeight = (int)(Window.ClientBounds.Width / DesignedResolutionAspectRatio + variance);
                var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height * DesignedResolutionAspectRatio + variance);
                var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            return scaleRectangle;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< HEAD
            _mainMenuBackground = Content.Load<Texture2D>("mainMenu");
            _splashScreen = Content.Load<Texture2D>("splashScreen2");
            _endLevelSummaryBackground = Content.Load<Texture2D>("endLevelSummary");
            _gameOverFont = Content.Load<SpriteFont>("GameOverFont");

            // Load the tileset texture
            var tilesetTexture = Content.Load<Texture2D>("SimpleTileset2");

            // Load the tiled map
            var map = new TmxMap("Content/JumpLand.tmx");
            int tilesetTilesWide = tilesetTexture.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;

            float scale = 2.0f; // Adjust the scale factor as needed
            _tileMapManager = new TileMapManager(_spriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight, scale);

            // Load player sprite sheet
            var playerSpriteSheet = Content.Load<Texture2D>("sonic"); // Assuming the sprite sheet is named 'sonic.png'

            // Load enemy sprite sheet
            var enemySpriteSheet = Content.Load<Texture2D>("dr-robotnik");

            // Ensure the player entity is not created multiple times
            if (_playerEntity == null)
            {
                _playerEntity = new PlayerSprite(playerSpriteSheet, physicsWorld);
                _enemyEntity = new EnemySprite(enemySpriteSheet);
                _entities = new List<BaseEntity> { _playerEntity, _enemyEntity };
            }

            _movementSystem = new MovementSystem();
            _audioManager = new AudioManager(Content); // Initialize _audioManager
            _collisionSystem = new CollisionSystem(_audioManager, this); // Pass the MainGame instance

            _inputManager = new InputManager(new GameplayInputMapper());

            // Initialize and load content for PhysicsBallState
            _physicsBallState = new PhysicsBallState();
            _physicsBallState.Initialize(Content, this);
            _physicsBallState.LoadContent();

            // Load collision objects from the Tiled map
            LoadCollisionObjects(map);
        }

        private void LoadCollisionObjects(TmxMap map)
        {
            collisionRectangles = new List<Rectangle>();
            foreach (var obj in map.ObjectGroups["Object Layer 1"].Objects)
            {
                if (obj.Width > 0 && obj.Height > 0)
                {
                    var rectangle = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                    collisionRectangles.Add(rectangle);

                    var body = physicsWorld.CreateRectangle(
                        ConvertUnits.ToSimUnits((float)obj.Width),
                        ConvertUnits.ToSimUnits((float)obj.Height),
                        1f,
                        ConvertUnits.ToSimUnits(new Vector2((float)(obj.X + obj.Width / 2f), (float)(obj.Y + obj.Height / 2f)))
                    );
                    body.BodyType = BodyType.Static;
                    body.UserData = "collisionObject"; // Set user data for collision handling
                }
            }
=======
            // Load common content if necessary
>>>>>>> main
        }

        protected override void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
            _currentState?.HandleInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _currentState?.Render(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

<<<<<<< HEAD
        private void HandleGameplayInput(KeyboardState currentKeyboardState)
        {
            if (_inputManager == null)
            {
                throw new InvalidOperationException("InputManager is not initialized.");
            }

            Vector2 movement = Vector2.Zero;
            if (currentKeyboardState.IsKeyDown(Keys.A))
                movement.X -= 5;
            if (currentKeyboardState.IsKeyDown(Keys.D))
                movement.X += 5;
            if (currentKeyboardState.IsKeyDown(Keys.Space) && playerBody.LinearVelocity.Y == 0)
                movement.Y = -_playerEntity.JumpSpeed;

            playerBody.ApplyLinearImpulse(movement);
        }

        private void HandleEndLevelSummaryInput(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                Exit();
            }
        }

        public void InitializeGameplay()
        {
            if (_playerEntity == null)
            {
                // Load player sprite sheet
                var playerSpriteSheet = Content.Load<Texture2D>("sonic"); // Assuming the sprite sheet is named 'sonic.png'
                _playerEntity = new PlayerSprite(playerSpriteSheet, physicsWorld);
                var enemySpriteSheet = Content.Load<Texture2D>("dr-robotnik-7");
                _enemyEntity = new EnemySprite(enemySpriteSheet);
                _entities = new List<BaseEntity> { _playerEntity, _enemyEntity };
            }

            _audioManager = new AudioManager(Content);
            _audioManager.LoadSound("donkeyKongHurt", "donkey-kong-hurt");
            _audioManager.LoadSong("jungleHijynx", "jungle-hijynx");
            _audioManager.PlaySong("jungleHijynx", true);

            _movementSystem = new MovementSystem();
            _collisionSystem = new CollisionSystem(_audioManager, this);

            _inputManager = new InputManager(new GameplayInputMapper());

            Console.WriteLine("Luigi is hot"); // This line was added as requested
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            physicsWorld.Step(deltaTime);

            // Update player position based on physics body
            var playerPosition = ConvertUnits.ToDisplayUnits(playerBody.Position);
            _playerEntity.Position = playerPosition;

            // Update systems
            _gravitySystem.Update(_entities, deltaTime);
            _jumpingSystem.Update(_entities, deltaTime);
            _buoyancySystem.Update(_entities, deltaTime);
            _frictionAndDragSystem.Update(_entities, deltaTime);
            _movementSystem.Update(_entities);
            _collisionSystem.Update(_entities);
            _playerEntity.Update(gameTime);
            _enemyEntity.Update(gameTime);
        }

        private bool IsLevelCompleted()
        {
            return _playerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }

=======
>>>>>>> main
        public void SwitchState(BaseGameState newState)
        {
            _currentState?.UnloadContent();
            _currentState = newState;
            _currentState.Initialize(Content, this);
            _currentState.LoadContent();
            _currentState.OnStateSwitched += HandleStateSwitched;
            _currentState.OnEventNotification += HandleEventNotification;
        }

        private void HandleStateSwitched(object sender, BaseGameState newState)
        {
            SwitchState(newState);
        }

        private void HandleEventNotification(object sender, Events e)
        {
            if (e == Events.GAME_QUIT)
            {
                Exit();
            }
        }
    }
}
