using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.Component;
using Kong_Engine.Objects;
using Kong_Engine.ECS.System;
using System.Collections.Generic;
using Kong_Engine.Input;
using Kong_Engine.States.Base;
using System;
using System.IO;
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

        private TopDownCollisionSystem _collisionSystem;

        public int MapWidth => 480;
        public int MapHeight => 320;

        protected override void LoadLevelContent()
        {
            _backgroundTexture = Content.Load<Texture2D>("qwest-quest/all/QuestQuestMap");
            _spriteSheet = Content.Load<Texture2D>("qwest-quest/RACCOONSPRITESHEET");
            _font = Content.Load<SpriteFont>("ScoreFont");

            AudioManager.LoadSong("pixel-plains", "qwest-quest/pixel-plains");
            AudioManager.PlaySong("pixel-plains", isRepeating: true);
        }

        protected override void InitializeEntities()
        {
            var game = (MainGame)Game;
            game.Graphics.PreferredBackBufferWidth = MapWidth;
            game.Graphics.PreferredBackBufferHeight = MapHeight;
            game.Graphics.ApplyChanges();

            _random = new Random();

            _player3 = new RaccoonSprite(_spriteSheet, 1f, AudioManager);
            Entities.Add(_player3);

            _terrainBackground = new TerrainBackground(_backgroundTexture, Vector2.Zero, isScrollingEnabled: false);

            // Generate collision rectangles from the CSV file
            var collisionRectangles = GenerateCollisionRectanglesFromCsv("Content/qwest-quest/QwestQuest_Collision.csv");

            TileMapManager = new TileMapManager(
                Content,
                SpriteBatch,
                new TmxMap("Content/qwest-quest/QuestQuestMap.tmx"),
                Content.Load<Texture2D>("qwest-quest/collisions"),
                16,
                20,
                20
            );

            // Assign the generated collision rectangles to the TileMapManager
            TileMapManager.CollisionRectangles.AddRange(collisionRectangles);

            _collisionSystem = new TopDownCollisionSystem(AudioManager, game, TileMapManager);
        }


        private List<Rectangle> GenerateCollisionRectanglesFromCsv(string csvFilePath)
        {
            var collisionRectangles = new List<Rectangle>();
            var tileWidth = 16; // Tile width in pixels
            var tileHeight = 16; // Tile height in pixels

            try
            {
                using (var reader = new StreamReader(csvFilePath))
                {
                    string line;
                    int y = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');

                        for (int x = 0; x < values.Length; x++)
                        {
                            if (int.TryParse(values[x], out int tileValue) && tileValue == 68)
                            {
                                // Create a collision rectangle for each "68" tile value
                                var rect = new Rectangle(
                                    x * tileWidth,  // X position in pixels
                                    y * tileHeight, // Y position in pixels
                                    tileWidth,      // Width of the rectangle
                                    tileHeight      // Height of the rectangle
                                );
                                collisionRectangles.Add(rect);
                            }
                        }
                        y++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading collision CSV file: {ex.Message}");
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

            _player3.Update(gameTime);

            _collisionSystem.Update(Entities);

            if (_player3.IsDead())
            {
                isGameOver = true;
                return;
            }

            if (IsLevelCompleted())
            {
                isLevelPassed = true;
                return;
            }

            if (_terrainBackground.IsScrollingEnabled)
            {
                _terrainBackground.UpdateBackgroundPosition(_player3.GetComponent<PositionComponent>().Position);
            }

            base.Update(gameTime);
        }

        protected override bool IsLevelCompleted()
        {
            return false;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _terrainBackground.Render(spriteBatch);
            _player3.Draw(spriteBatch, Matrix.Identity);

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
