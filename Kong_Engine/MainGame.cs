using System;
using System.Collections.Generic;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.System;
using Kong_Engine.Enum;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.Objects.Base;
using Kong_Engine.States;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;

namespace Kong_Engine
{
    public enum GameState
    {
        SplashScreen,
        MainMenu,
        Gameplay,
        EndLevelSummary,
        GameOver
    }

    public class MainGame : Game
    {
        private GameState _currentGameState;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _renderScaleRectangle;
        private const int DesignedResolutionWidth = 1280;
        private const int DesignedResolutionHeight = 720;
        private const float DesignedResolutionAspectRatio = DesignedResolutionWidth / (float)DesignedResolutionHeight;

        // Gameplay variables
        private List<BaseEntity> _entities;
        private MovementSystem _movementSystem;
        private CollisionSystem _collisionSystem;
        private PlayerSprite _playerEntity;
        private EnemySprite _enemyEntity;
        private AudioManager _audioManager;
        private KeyboardState _previousKeyboardState;
        private InputManager _inputManager;
        private Texture2D _mainMenuBackground;
        private Texture2D _splashScreen;
        private Texture2D _endLevelSummaryBackground;
        private SpriteFont _gameOverFont;

        // Tiled map variables
        private TileMapManager _tileMapManager;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _currentGameState = GameState.SplashScreen;
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

            base.Initialize();
        }

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
                _playerEntity = new PlayerSprite(playerSpriteSheet);
                _enemyEntity = new EnemySprite(enemySpriteSheet);
                _entities = new List<BaseEntity> { _playerEntity, _enemyEntity };
            }

            _movementSystem = new MovementSystem();
            _audioManager = new AudioManager(Content); // Initialize _audioManager
            _collisionSystem = new CollisionSystem(_audioManager, this); // Pass the MainGame instance

            _inputManager = new InputManager(new GameplayInputMapper());
        }

        protected override void Update(GameTime gameTime)
        {
            var currentKeyboardState = Keyboard.GetState();

            switch (_currentGameState)
            {
                case GameState.SplashScreen:
                    if (currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        _currentGameState = GameState.MainMenu;
                    }
                    break;

                case GameState.MainMenu:
                    if (currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        InitializeGameplay();
                        _currentGameState = GameState.Gameplay;
                    }
                    break;

                case GameState.Gameplay:
                    HandleGameplayInput(currentKeyboardState);
                    UpdateGameplay(gameTime);
                    if (IsLevelCompleted())
                    {
                        _currentGameState = GameState.EndLevelSummary;
                    }
                    break;

                case GameState.EndLevelSummary:
                    HandleEndLevelSummaryInput(currentKeyboardState);
                    break;

                case GameState.GameOver:
                    if (currentKeyboardState.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        Exit(); // Exit the game or reset the game
                    }
                    break;
            }

            _previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_currentGameState)
            {
                case GameState.SplashScreen:
                    _spriteBatch.Draw(_splashScreen, new Vector2(0, 0), Color.White);
                    break;

                case GameState.MainMenu:
                    _spriteBatch.Draw(_mainMenuBackground, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Gameplay:
                    // Existing drawing code for gameplay...

                    // Drawing tile map
                    var transformMatrix = Matrix.CreateScale(1); // No additional scaling here
                    _spriteBatch.End(); // End current Begin
                    _tileMapManager.Draw(transformMatrix); // Draw the tile map with its own Begin and End

                    // Draw player and enemy entities with the same scaling factor
                    _playerEntity.Draw(_spriteBatch, transformMatrix);
                    _enemyEntity.Draw(_spriteBatch, transformMatrix);

                    _spriteBatch.Begin(); // Begin again for subsequent drawings

                    // Add any additional drawing calls here

                    break;

                case GameState.EndLevelSummary:
                    _spriteBatch.Draw(_endLevelSummaryBackground, new Vector2(0, 0), Color.White);
                    break;

                case GameState.GameOver:
                    var gameOverText = "Game Over";
                    var textSize = _gameOverFont.MeasureString(gameOverText);
                    var position = new Vector2((DesignedResolutionWidth - textSize.X) / 2, (DesignedResolutionHeight - textSize.Y) / 2);
                    _spriteBatch.DrawString(_gameOverFont, gameOverText, position, Color.Red);
                    break;
            }

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

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
            if (currentKeyboardState.IsKeyDown(Keys.W))
                movement.Y -= 5;
            if (currentKeyboardState.IsKeyDown(Keys.S))
                movement.Y += 5;

            _playerEntity.Move(movement);
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
                _playerEntity = new PlayerSprite(playerSpriteSheet);
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
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            _movementSystem.Update(_entities);
            _collisionSystem.Update(_entities);
            _playerEntity.Update(gameTime);
            _enemyEntity.Update(gameTime);
        }

        private bool IsLevelCompleted()
        {
            return _playerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }

        public void SwitchState(BaseGameState newState)
        {
            newState.Initialize(Content, this);
            newState.OnStateSwitched += OnStateSwitchedHandler;
            newState.OnEventNotification += OnEventNotificationHandler;
            newState.LoadContent();
            _currentGameState = GameState.GameOver;
        }

        private void OnStateSwitchedHandler(object sender, BaseGameState newState)
        {
            SwitchState(newState);
        }

        private void OnEventNotificationHandler(object sender, Events e)
        {
            if (e == Events.GAME_QUIT)
            {
                Exit();
            }
        }
    }
}
