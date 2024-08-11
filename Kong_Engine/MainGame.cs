using Kong_Engine.ECS.Entity;
using Kong_Engine.Enum;
using Kong_Engine.Objects;
using Kong_Engine.States;
using Kong_Engine.States.Base;
using Kong_Engine.States.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        private float _scaleFactor;

        // Declare the _camera variable here
        private Camera _camera;

        public float ScaleFactor => _scaleFactor;

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
            _audioManager = new AudioManager(Content);

            // Initialize the camera here
            _camera = new Camera(GraphicsDevice.Viewport);

            // Start with the Splash Screen
            SwitchState(new SplashState());
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
        }

        protected override void Update(GameTime gameTime)
        {
            // Move the camera based on input
            Vector2 moveDirection = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                moveDirection.X -= 5f; // Move left
            if (Keyboard.GetState().IsKeyDown(Keys.L))
                moveDirection.X += 5f; // Move right
            if (Keyboard.GetState().IsKeyDown(Keys.I))
                moveDirection.Y -= 5f; // Move up
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                moveDirection.Y += 5f; // Move down

            _camera.Move(moveDirection);

            _currentState?.Update(gameTime);
            _currentState?.HandleInput();
            _camera.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: _camera.Transform); // Apply camera transformation
            {
                _currentState?.Render(_spriteBatch);
            }
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            _spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
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
