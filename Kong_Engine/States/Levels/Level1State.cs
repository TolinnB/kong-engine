using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.System;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Kong_Engine.States.Levels
{
    public class Level1State : BaseLevelState
    {
        protected override void LoadLevelContent()
        {
            var tilesetTexture = Content.Load<Texture2D>("SimpleTileset2");
            var map = new TmxMap("Content/JumpLand.tmx");
            int tilesetTilesWide = tilesetTexture.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            float scale = 4.0f;

            TileMapManager = new TileMapManager(SpriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight, scale);
        }

        protected override void InitializeEntities()
        {
            var playerSpriteSheet = Content.Load<Texture2D>("player");
            var enemySpriteSheet = Content.Load<Texture2D>("slime");
            float scale = 3.0f;

            PlayerEntity = new PlayerSprite(playerSpriteSheet, scale);
            EnemyEntity = new EnemySprite(enemySpriteSheet, scale);

            Entities.Add(PlayerEntity);
            Entities.Add(EnemyEntity);
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public override void LoadContent()
        {
            base.LoadContent();
            AudioManager.LoadSound("hurtSound", "hurtSound");
            AudioManager.LoadSong("music", "music");
            AudioManager.PlaySong("music", true);
        }

        protected override bool IsLevelCompleted()
        {
            // Add your level completion logic here
            // For example, return true if player reaches a certain position
            return PlayerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);
            int screenWidth = game.GraphicsDevice.Viewport.Width; // Get the screen width
            MovementSystem = new MovementSystem(screenWidth); // Initialize with screen width
        }

        public override void Update(GameTime gameTime)
        {
            MovementSystem.Update(Entities);
            CollisionSystem.Update(Entities);
            PlayerEntity?.Update(gameTime);
            EnemyEntity?.Update(gameTime);

            if (IsLevelCompleted())
            {
                SwitchState(new EndLevelSummaryState());
            }

            base.Update(gameTime);
        }
    }
}
