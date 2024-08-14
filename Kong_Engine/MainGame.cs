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

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            _audioManager = new AudioManager(Content);

            // Start with the initial state (Level3State in this case)
            SwitchState(new Level3State());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Set window size to match the size of the map
            if (_currentState is Level3State levelState)
            {
                var mapWidth = levelState.MapWidth;
                var mapHeight = levelState.MapHeight;

                _graphics.PreferredBackBufferWidth = mapWidth;
                _graphics.PreferredBackBufferHeight = mapHeight;
                _graphics.ApplyChanges();

                // Ensure the window size matches the content
                Window.AllowUserResizing = false;
                _graphics.HardwareModeSwitch = false;
                _graphics.IsFullScreen = false;
                _graphics.ApplyChanges();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
            _currentState?.HandleInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); // Black background to avoid any borders

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
