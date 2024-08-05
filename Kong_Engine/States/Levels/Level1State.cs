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
            float scale = 1.0f; // Adjust the scale factor as needed

            TileMapManager = new TileMapManager(SpriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight, scale);
        }

        protected override void InitializeEntities()
        {
            var playerSpriteSheet = Content.Load<Texture2D>("player");
            var enemySpriteSheet = Content.Load<Texture2D>("slime");
            float playerScale = 1.0f; // Adjust player scale as needed
            float enemyScale = 1.0f;  // Adjust enemy scale as needed

            PlayerEntity = new PlayerSprite(playerSpriteSheet, TileMapManager, playerScale);
            EnemyEntity = new EnemySprite(enemySpriteSheet, enemyScale);

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
            return PlayerEntity.GetComponent<PositionComponent>().Position.X > 1000;
        }

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);
            int screenWidth = game.GraphicsDevice.Viewport.Width;
            MovementSystem = new MovementSystem(screenWidth);
            CollisionSystem = new CollisionSystem(AudioManager, game, TileMapManager); // Initialize CollisionSystem
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
