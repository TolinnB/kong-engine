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
        EndLevelSummary
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
        private AudioManager _audioManager;
        private KeyboardState _previousKeyboardState;
        private InputManager _inputManager;
        private Texture2D _mainMenuBackground;
        private Texture2D _splashScreen;
        private Texture2D _endLevelSummaryBackground;

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
            _splashScreen = Content.Load<Texture2D>("splashScreen");
            _endLevelSummaryBackground = Content.Load<Texture2D>("endLevelSummary");

            // Load the tileset texture
            var tilesetTexture = Content.Load<Texture2D>("env");

            // Load the tiled map
            var map = new TmxMap("Content/map.tmx");
            int tilesetTilesWide = tilesetTexture.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;

            _tileMapManager = new TileMapManager(_spriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight);

            // Load player textures
            Texture2D[] idleTextures = new Texture2D[]
            {
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_down"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_left"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_right"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_up")
            };

            Texture2D[] walkTextures = new Texture2D[]
            {
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_down"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_left"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_right"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_up")
            };

            // Ensure the player entity is not created multiple times
            if (_playerEntity == null)
            {
                _playerEntity = new PlayerSprite(idleTextures, walkTextures);
                _entities = new List<BaseEntity> { _playerEntity };
            }

            _movementSystem = new MovementSystem();
            _collisionSystem = new CollisionSystem(_audioManager);

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
                    // Drawing tile map
                    var transformMatrix = Matrix.CreateScale(1); // Adjust scale if needed
                    _spriteBatch.End(); // End current Begin
                    _tileMapManager.Draw(transformMatrix); // Draw the tile map with its own Begin and End

                    // Draw player
                    _playerEntity.Draw(_spriteBatch, transformMatrix);

                    _spriteBatch.Begin(); // Begin again for subsequent drawings

                    // Add any additional drawing calls here

                    break;

                case GameState.EndLevelSummary:
                    _spriteBatch.Draw(_endLevelSummaryBackground, new Vector2(0, 0), Color.White);
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
            _audioManager = new AudioManager(Content);
            _audioManager.LoadSound("donkeyKongHurt", "donkey-kong-hurt");
            _audioManager.LoadSong("jungleHijynx", "jungle-hijynx");
            _audioManager.PlaySong("jungleHijynx", true);

            // Ensure to update the player textures here
            Texture2D[] idleTextures = new Texture2D[]
            {
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_down"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_left"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_right"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Idle/Char_idle_up")
            };

            Texture2D[] walkTextures = new Texture2D[]
            {
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_down"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_left"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_right"),
        Content.Load<Texture2D>("Tiny Adventure Pack/Character/Char_one/Walk/Char_walk_up")
            };

            // Ensure the player entity is not re-created
            if (_playerEntity == null)
            {
                _playerEntity = new PlayerSprite(idleTextures, walkTextures);
                _entities = new List<BaseEntity> { _playerEntity };
            }

            _movementSystem = new MovementSystem();
            _collisionSystem = new CollisionSystem(_audioManager);

            _inputManager = new InputManager(new GameplayInputMapper());
        }


        private void UpdateGameplay(GameTime gameTime)
        {
            _movementSystem.Update(_entities);
            _collisionSystem.Update(_entities);
            _playerEntity.Update(gameTime);
        }

        private bool IsLevelCompleted()
        {
            return _playerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }
    }
}
