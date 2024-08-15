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
        //Variables 

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BaseGameState _currentState;
        private AudioManager _audioManager;
        public float ScaleFactor { get; set; } = 1.0f;
        public GraphicsDeviceManager Graphics => _graphics;

        // Default Window Size
        private readonly int _defaultWidth = 1280;
        private readonly int _defaultHeight = 720;

        //Begins Game
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Initialises the audio manager and shifts to Splash Screen
        protected override void Initialize()
        {
            base.Initialize();
            _audioManager = new AudioManager(Content);

            // Start with the initial state (SplashState in this case)
            SwitchState(new Level3State());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            AdjustWindowSize();
        }

        //Update the game logic, sprites or inputs
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

        /**/
        // Transitions between states and unloads content before setting up handlers for the new state
        /**/
        public void SwitchState(BaseGameState newState)
        {
            _currentState?.UnloadContent();
            _currentState = newState;
            _currentState.Initialize(Content, this);
            _currentState.LoadContent();
            _currentState.OnStateSwitched += HandleStateSwitched;
            _currentState.OnEventNotification += HandleEventNotification;

            // Adjust the window size after switching the state
            AdjustWindowSize();
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

        private void AdjustWindowSize()
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
                _graphics.PreferredBackBufferWidth = _defaultWidth;
                _graphics.PreferredBackBufferHeight = _defaultHeight;
            }

            _graphics.ApplyChanges();
        }
    }
}
