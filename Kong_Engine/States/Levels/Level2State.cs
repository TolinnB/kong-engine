using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Kong_Engine.Objects;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using Kong_Engine.Input;
using Kong_Engine.States.Base;
using System;

namespace Kong_Engine.States.Levels
{
    public class Level2State : BaseLevelState
    {
        private Texture2D _backgroundTexture;
        private Texture2D _spriteSheet;
        private SpriteFont _font; // Font for displaying the score
        private PlayerSprite2 _player2;
        private List<Asteroid> _asteroids;
        private TerrainBackground _terrainBackground;
        private Random _random;
        private bool isGameOver = false;
        private bool isLevelPassed = false;

        protected override void LoadLevelContent()
        {
            _backgroundTexture = Content.Load<Texture2D>("space");
            _spriteSheet = Content.Load<Texture2D>("space-sprites"); // Load the provided sprite sheet without the extension
            _font = Content.Load<SpriteFont>("ScoreFont"); // Load the font for displaying the score

            // Load the space-music song
            AudioManager.LoadSong("space-music", "space-music");
            AudioManager.PlaySong("space-music", true);
        }
        protected override void InitializeEntities()
        {
            _random = new Random();

            // Initialize PlayerSprite2
            _player2 = new PlayerSprite2(_spriteSheet, 1f);
            Entities.Add(_player2);

            // Initialize TerrainBackground with a scroll speed (e.g., 0.5 for half the speed of the player)
            _terrainBackground = new TerrainBackground(_backgroundTexture, new Vector2(0.5f, 0.5f));

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
            // Level is passed if there are no more asteroids
            return _asteroids.Count == 0;
        }

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);

            // Play the space-music song
            AudioManager.PlaySong("space-music", true); // Play with repeating enabled
        }


        public override void Update(GameTime gameTime)
        {
            if (isGameOver || isLevelPassed)
                return;

            // Update PlayerSprite2
            _player2.Update(gameTime, _asteroids);

            // Check if the player is dead
            if (_player2.IsDead())
            {
                isGameOver = true;
                return;
            }

            // Check if the level is passed
            if (IsLevelCompleted())
            {
                isLevelPassed = true;
                return;
            }

            // Update TerrainBackground based on player's position
            _terrainBackground.UpdateBackgroundPosition(_player2.GetComponent<PositionComponent>().Position);

            // Update Asteroids
            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Update(gameTime);

                if (_asteroids[i].MarkedForRemoval)
                {
                    _asteroids.RemoveAt(i);
                }
                else
                {
                    // Check for collisions
                    if (_asteroids[i].GetBoundingBox().Intersects(_player2.GetBoundingBox()))
                    {
                        Debug.WriteLine("Collision detected!");
                        _player2.HandleCollision(new Vector2(0, 10)); // Apply downward knockback
                        // Handle other asteroid-specific collision logic if needed
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _terrainBackground.Render(spriteBatch);

            // Draw PlayerSprite2
            _player2.Draw(spriteBatch, Matrix.Identity);
            // Draw Asteroids
            foreach (var asteroid in _asteroids)
            {
                asteroid.Draw(spriteBatch);
            }

            // Draw the score
            spriteBatch.DrawString(_font, $"Score: {_player2.Score}", new Vector2(10, 10), Color.White);

            if (isGameOver)
            {
                var gameOverText = "Game Over";
                var gameOverPosition = new Vector2(
                    (spriteBatch.GraphicsDevice.Viewport.Width - _font.MeasureString(gameOverText).X) / 2,
                    (spriteBatch.GraphicsDevice.Viewport.Height - _font.MeasureString(gameOverText).Y) / 2
                );
                spriteBatch.DrawString(_font, gameOverText, gameOverPosition, Color.Red);
            }

            if (isLevelPassed)
            {
                var levelPassedText = $"Level Passed! Score: {_player2.Score}";
                var levelPassedPosition = new Vector2(
                    (spriteBatch.GraphicsDevice.Viewport.Width - _font.MeasureString(levelPassedText).X) / 2,
                    (spriteBatch.GraphicsDevice.Viewport.Height - _font.MeasureString(levelPassedText).Y) / 2
                );
                spriteBatch.DrawString(_font, levelPassedText, levelPassedPosition, Color.Green);
            }
        }
    }
}
