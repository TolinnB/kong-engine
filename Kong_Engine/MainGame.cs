using Kong_Engine.ECS.Entity;
using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States;
using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BaseGameState _currentState;
        private AudioManager _audioManager;
        public float ScaleFactor { get; set; } = 1.0f;  // Default scale factor
        public GraphicsDeviceManager Graphics => _graphics;

        // Store the default window size
        private readonly int _defaultWidth = 1280;
        private readonly int _defaultHeight = 720;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            _audioManager = new AudioManager(Content);

            // Start with the initial state (SplashState in this case)
            SwitchState(new SplashState());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            AdjustWindowSizeForCurrentState();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
            _currentState?.HandleInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with the color that matches the background color of your map
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _currentState?.Render(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SwitchState(BaseGameState newState)
        {
            _currentState?.UnloadContent();
            _currentState = newState;
            _currentState.Initialize(Content, this);
            _currentState.LoadContent();
            _currentState.OnStateSwitched += HandleStateSwitched;
            _currentState.OnEventNotification += HandleEventNotification;

            // Adjust the window size after switching the state
            AdjustWindowSizeForCurrentState();
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

        private void AdjustWindowSizeForCurrentState()
        {
            // Check if the current state is Level3State
            if (_currentState is Level3State levelState)
            {
                var mapWidth = levelState.MapWidth;
                var mapHeight = levelState.MapHeight;

                _graphics.PreferredBackBufferWidth = mapWidth;
                _graphics.PreferredBackBufferHeight = mapHeight;
            }
            else
            {
                // Set to default window size for other states
                _graphics.PreferredBackBufferWidth = _defaultWidth;
                _graphics.PreferredBackBufferHeight = _defaultHeight;
            }

            _graphics.ApplyChanges();
        }
    }
}
