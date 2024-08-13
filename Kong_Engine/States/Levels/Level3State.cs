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
using TiledSharp;

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

        private int[,] collisionGrid = new int[,]
{
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {68,68,68,68,68,68,68,68,68,68,68,68,68,68,68,68,68,68,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,68,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,68,68,68,68,68,68,68,68,68,-1,-1,-1,-1,-1,-1,68,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,68,68,68,68,68,68,-1,-1,68,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,68,-1,-1,68,68,68},
    {-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,68,-1,-1},
    {-1,-1,-1,68,68,68,68,68,68,68,68,68,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,68,68,68,68,68,68,68,68,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1,-1,-1,-1,-1,-1,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,68,68,68,68,68,68,-1,-1,68,68,-1,-1,-1},
    {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}
};


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

            // Generate collision rectangles from the grid
            var collisionRectangles = GenerateCollisionRectangles();

            // Debug: Print collision rectangles
            Console.WriteLine("Collision Rectangles:");
            foreach (var rect in collisionRectangles)
            {
                Console.WriteLine($"Rectangle: X={rect.X}, Y={rect.Y}, Width={rect.Width}, Height={rect.Height}");
            }

            // Initialize TileMapManager with the correct path to your map
            TileMapManager = new TileMapManager(
                SpriteBatch,
                new TmxMap("Content/qwest-quest/QuestQuestMap.tmx"), // Corrected path to the .tmx file
                Content.Load<Texture2D>("qwest-quest/collisions"), // Load your tileset texture
                16,
                20,
                20,
                collisionRectangles
            );
        }

        private List<Rectangle> GenerateCollisionRectangles()
        {
            var tileWidth = 20; // Assuming each grid cell corresponds to a 20x20 pixel area
            var tileHeight = 20;

            var collisionRectangles = new List<Rectangle>();

            for (int y = 0; y < collisionGrid.GetLength(0); y++)
            {
                for (int x = 0; x < collisionGrid.GetLength(1); x++)
                {
                    if (collisionGrid[y, x] == 68)
                    {
                        var rect = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                        collisionRectangles.Add(rect);
                    }
                }
            }

            return collisionRectangles;
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
            return false; // Placeholder
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _terrainBackground.Render(spriteBatch);

            // Draw RaccoonSprite3
            _player3.Draw(spriteBatch, Matrix.Identity);

            // Draw other entities like enemies, NPCs, etc.

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
