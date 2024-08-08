using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using System;

namespace Kong_Engine.States.Levels
{
    public class Level2State : BaseLevelState
    {
        private Texture2D _backgroundTexture;
        private Texture2D _spriteSheet;
        private PlayerSprite2 _player2;
        private List<Asteroid> _asteroids;
        private Random _random;

        protected override void LoadLevelContent()
        {
            _backgroundTexture = Content.Load<Texture2D>("space");
            _spriteSheet = Content.Load<Texture2D>("space-sprites"); // Load the provided sprite sheet without the extension
        }

        protected override void InitializeEntities()
        {
            _random = new Random();

            // Initialize PlayerSprite2
            _player2 = new PlayerSprite2(_spriteSheet, 1f);
            Entities.Add(_player2);

            // Initialize multiple asteroids
            _asteroids = new List<Asteroid>();
            int numberOfAsteroids = 5;
            for (int i = 0; i < numberOfAsteroids; i++)
            {
                var asteroidPosition = new Vector2(_random.Next(0, 1280), _random.Next(0, 50)); // Random X position, slight variance in Y position
                var asteroidVelocity = new Vector2(0, 50); // Move downward
                var asteroid = new Asteroid(_spriteSheet, asteroidPosition, 1f, asteroidVelocity);
                _asteroids.Add(asteroid);
                Entities.Add(asteroid);

                // Log asteroid initialization
                Debug.WriteLine($"Asteroid {i} added to entities at position {asteroidPosition}");
            }
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
            // Update Asteroids
            foreach (var asteroid in _asteroids)
            {
                asteroid.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);

            // Draw PlayerSprite2
            _player2.Draw(spriteBatch, Matrix.Identity);
            // Draw Asteroids
            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }
    }
}
