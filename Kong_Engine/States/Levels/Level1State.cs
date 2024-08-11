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
        private float globalScale = 2.0f; // Example scale factor
        private GravitySystem gravitySystem;

        protected override void LoadLevelContent()
        {
            var tilesetTexture = Content.Load<Texture2D>("SimpleTileset2");
            var map = new TmxMap("Content/JumpLand.tmx");
            int tilesetTilesWide = tilesetTexture.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;

            // Pass ContentManager when creating TileMapManager
            TileMapManager = new TileMapManager(Content, SpriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight, globalScale);
        }

        protected override void InitializeEntities()
        {
            var playerSpriteSheet = Content.Load<Texture2D>("player");
            var enemySpriteSheet = Content.Load<Texture2D>("slime");

            PlayerEntity = new PlayerSprite(playerSpriteSheet, TileMapManager, globalScale);
            EnemyEntity = new EnemySprite(enemySpriteSheet, globalScale);

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
            return PlayerEntity.GetComponent<PositionComponent>().Position.X > 1000 * globalScale;
        }

        public override void Initialize(ContentManager contentManager, MainGame game)
        {
            base.Initialize(contentManager, game);
            int screenWidth = game.GraphicsDevice.Viewport.Width;
            MovementSystem = new MovementSystem(screenWidth);
            CollisionSystem = new CollisionSystem(AudioManager, game, TileMapManager); // Initialize CollisionSystem
            gravitySystem = new GravitySystem(new Vector2(0, 9.8f));
        }

        public override void Update(GameTime gameTime)
        {
            MovementSystem.Update(Entities);
            CollisionSystem.Update(Entities);
            PlayerEntity?.Update(gameTime);
            EnemyEntity?.Update(gameTime);
            gravitySystem.Update(Entities, (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (IsLevelCompleted())
            {
                SwitchState(new Level2State());
            }

            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Matrix scaleMatrix = Matrix.CreateScale(globalScale);
            TileMapManager?.Draw(scaleMatrix);
            PlayerEntity?.Draw(spriteBatch, scaleMatrix);
            EnemyEntity?.Draw(spriteBatch, scaleMatrix);
        }
    }
}
