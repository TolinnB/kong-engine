using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;

namespace Kong_Engine.States.Levels
{
    public class Level2State : BaseLevelState
    {
        private Texture2D _backgroundTexture;
        private Texture2D _spriteSheet;
        private PlayerSprite2 _player2;
        private Asteroid _asteroid;

        protected override void LoadLevelContent()
        {
            _backgroundTexture = Content.Load<Texture2D>("space");
            _spriteSheet = Content.Load<Texture2D>("space-sprites"); // Load the provided sprite sheet without the extension
        }

        protected override void InitializeEntities()
        {
            // Initialize PlayerSprite2
            _player2 = new PlayerSprite2(_spriteSheet, 1f);
            Entities.Add(_player2);

            // Initialize Asteroid
            var asteroidPosition = new Vector2(300, 200);
            _asteroid = new Asteroid(_spriteSheet, asteroidPosition, 1f, new Vector2(50, 0)); // Example velocity
            Entities.Add(_asteroid);

            // Log asteroid initialization
            Debug.WriteLine("Asteroid added to entities");
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public override void LoadContent()
        {
            base.LoadContent();
            // No additional content for now
        }

        protected override bool IsLevelCompleted()
        {
            return false; // For now, the level doesn't complete
        }

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);
        }

        public override void Update(GameTime gameTime)
        {
            // Update PlayerSprite2
            _player2.Update(gameTime);
            // Update Asteroid
            _asteroid.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);

            // Draw PlayerSprite2
            _player2.Draw(spriteBatch, Matrix.Identity);
            // Draw Asteroid
            _asteroid.Draw(spriteBatch);
        }
    }
}
