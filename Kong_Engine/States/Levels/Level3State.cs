using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Kong_Engine.Objects;
using System.Collections.Generic;
using Kong_Engine.Input;
using Kong_Engine.States.Base;
using System;

namespace Kong_Engine.States.Levels
{
    public class Level3State : BaseLevelState
    {
        private Texture2D _backgroundTexture;
        private Texture2D _spriteSheet;
        private SpriteFont _font; // Font for displaying the score or other messages
        private RaccoonSprite _player3;
        private TerrainBackground _terrainBackground;
        private Random _random;
        private bool isGameOver = false;
        private bool isLevelPassed = false;

        protected override void LoadLevelContent()
        {
            _backgroundTexture = Content.Load<Texture2D>("qwest-quest/all/QuestQuestMap"); // Load the overworld texture for the background
            _spriteSheet = Content.Load<Texture2D>("qwest-quest/RACCOONSPRITESHEET"); // Load the sprite sheet for the player or other entities
            _font = Content.Load<SpriteFont>("ScoreFont"); // Load font for displaying messages
        }

        protected override void InitializeEntities()
        {
            _random = new Random();

            // Initialize RaccoonSprite3 (assuming this is the player's class for Level 3)
            _player3 = new RaccoonSprite(_spriteSheet, 1f, AudioManager);
            Entities.Add(_player3);

            // Initialize TerrainBackground with a static background (no scrolling)
            _terrainBackground = new TerrainBackground(_backgroundTexture, Vector2.Zero, isScrollingEnabled: false);

            // Initialize any additional entities like enemies, NPCs, or objects
            // For example:
            // var enemy = new EnemySprite(_spriteSheet, new Vector2(100, 100), 1f, AudioManager);
            // Entities.Add(enemy);
        }


        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public override void Update(GameTime gameTime)
        {
            if (isGameOver || isLevelPassed)
                return;

            // Update RaccoonSprite3
            _player3.Update(gameTime);

            // Check if the player is dead
            if (_player3.IsDead())
            {
                isGameOver = true;
                return;
            }

            // Check if the level is passed (implement your own logic)
            if (IsLevelCompleted())
            {
                isLevelPassed = true;
                return;
            }

            // If scrolling is enabled, update TerrainBackground based on player's position
            if (_terrainBackground.IsScrollingEnabled)
            {
                _terrainBackground.UpdateBackgroundPosition(_player3.GetComponent<PositionComponent>().Position);
            }

            // Update other entities as necessary

            base.Update(gameTime);
        }


        protected override bool IsLevelCompleted()
        {
            // Implement logic for determining if the level is completed
            // For example, if the player reaches a specific point or defeats all enemies
            return false; // Placeholder
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _terrainBackground.Render(spriteBatch);

            // Draw RaccoonSprite3
            _player3.Draw(spriteBatch, Matrix.Identity);

            // Draw other entities like enemies, NPCs, etc.

            // Draw any HUD elements or messages
            //spriteBatch.DrawString(_font, "HP: " + _player3.Health, new Vector2(10, 10), Color.White);

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
                var levelPassedText = "Level Passed!";
                var levelPassedPosition = new Vector2(
                    (spriteBatch.GraphicsDevice.Viewport.Width - _font.MeasureString(levelPassedText).X) / 2,
                    (spriteBatch.GraphicsDevice.Viewport.Height - _font.MeasureString(levelPassedText).Y) / 2
                );
                spriteBatch.DrawString(_font, levelPassedText, levelPassedPosition, Color.Green);
            }
        }
    }
}
