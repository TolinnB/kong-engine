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
        private BaseEntity _playerEntity;
        private AudioManager _audioManager;
        private TerrainBackground _background;
        private KeyboardState _previousKeyboardState;
        private InputManager _inputManager;
        private Texture2D _mainMenuBackground;
        private Texture2D _splashScreen;
        private Texture2D _endLevelSummaryBackground;

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

            // Load additional content for gameplay here if needed
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
                    RenderGameplay(_spriteBatch);
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

            _inputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(-5, 0);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(5, 0);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveUp)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(0, -5);
                }
                else if (cmd is GameplayInputCommand.PlayerMoveDown)
                {
                    _playerEntity.GetComponent<PositionComponent>().Position += new Vector2(0, 5);
                }
            });
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
            var backgroundTexture = LoadTexture("DKJunglejpg");
            _background = new TerrainBackground(backgroundTexture);

            _audioManager = new AudioManager(Content);
            _audioManager.LoadSound("donkeyKongHurt", "donkey-kong-hurt");
            _audioManager.LoadSong("jungleHijynx", "jungle-hijynx");
            _audioManager.PlaySong("jungleHijynx", true);

            _playerEntity = new PlayerSprite(LoadTexture("donkeyKong"));
            _entities = new List<BaseEntity> { _playerEntity };

            var enemyTexture = LoadTexture("kingKRool");
            var enemyEntity = new EnemySprite(enemyTexture);
            _entities.Add(enemyEntity);

            _movementSystem = new MovementSystem();
            _collisionSystem = new CollisionSystem(_audioManager);

            AddGameObject(new EntityGameObjectAdapter(_playerEntity));
            AddGameObject(new EntityGameObjectAdapter(enemyEntity));

            _inputManager = new InputManager(new GameplayInputMapper());
            _previousKeyboardState = Keyboard.GetState();
        }

        private void UpdateGameplay(GameTime gameTime)
        {
            _movementSystem.Update(_entities);
            _collisionSystem.Update(_entities);
            (_playerEntity as PlayerSprite)?.Update();
            _background.UpdateBackgroundPosition(_playerEntity.GetComponent<PositionComponent>().Position);
        }

        private void RenderGameplay(SpriteBatch spriteBatch)
        {
            _background.Render(spriteBatch);
            foreach (var entity in _entities)
            {
                if (entity.HasComponent<TextureComponent>())
                {
                    var textureComponent = entity.GetComponent<TextureComponent>();
                    var positionComponent = entity.GetComponent<PositionComponent>();
                    spriteBatch.Draw(textureComponent.Texture, positionComponent.Position, Color.White);
                }
            }
        }

        private Texture2D LoadTexture(string textureName)
        {
            var texture = Content.Load<Texture2D>(textureName);
            return texture ?? Content.Load<Texture2D>("fallbackTexture");
        }

        private void AddGameObject(BaseGameObject gameObject)
        {
            // Add logic for adding game objects to the MainGame's collection
        }

        private bool IsLevelCompleted()
        {
            // Implement your logic to determine if the level is completed
            // This is a placeholder implementation
            return _playerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }
    }
}
