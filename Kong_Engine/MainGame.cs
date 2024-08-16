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
        // Variables 

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BaseGameState _currentState;
        private AudioManager _audioManager;
        private float _scaleFactor = 1.0f;

        public GraphicsDeviceManager GraphicsManager => _graphics;
        public AudioManager AudioManager => _audioManager;
        public BaseGameState CurrentState => _currentState; // Added property to expose _currentState

        // Default Window Size
        private readonly int _defaultWidth = 1280;
        private readonly int _defaultHeight = 720;

        // Begins Game
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set the preferred window size
            _graphics.PreferredBackBufferWidth = _defaultWidth;
            _graphics.PreferredBackBufferHeight = _defaultHeight;
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            _audioManager = new AudioManager(Content);
        }

        // Initialises the audio manager and shifts to Splash Screen
        protected override void Initialize()
        {
            base.Initialize();
            _audioManager = new AudioManager(Content);

            // Start with the initial state (SplashState in this case)
            SwitchState(new SplashState());
        }

        public void InitializeGame()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // Update the game logic, sprites or inputs
        protected override void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
            _currentState?.HandleInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _currentState?.Render(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Transitions between states and unloads content before setting up handlers for the new state
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

        // Handle quitting the game
        private void HandleEventNotification(object sender, Events e)
        {
            if (e == Events.GAME_QUIT)
            {
                Exit();
            }
        }

        // Property to access ScaleFactor
        public float ScaleFactor
        {
            get { return _scaleFactor; }
            set { _scaleFactor = value; }
        }
    }
}
