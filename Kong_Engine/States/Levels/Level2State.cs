using Kong_Engine.ECS.Component;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Kong_Engine.States.Levels
{
    public class Level2State : BaseLevelState
    {
        protected override void LoadLevelContent()
        {
            var tilesetTexture = Content.Load<Texture2D>("SimpleTileset2"); // Update the texture name if necessary
            var map = new TmxMap("Content/JumpLand2.tmx"); // Assuming the second level's Tiled map file
            int tilesetTilesWide = tilesetTexture.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            float scale = 2.0f;

            TileMapManager = new TileMapManager(SpriteBatch, map, tilesetTexture, tilesetTilesWide, tileWidth, tileHeight, scale);
        }

        protected override void InitializeEntities()
        {
            var playerSpriteSheet = Content.Load<Texture2D>("sonic");
            var enemySpriteSheet = Content.Load<Texture2D>("dr-robotnik");

            PlayerEntity = new PlayerSprite(playerSpriteSheet);
            EnemyEntity = new EnemySprite(enemySpriteSheet);

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
            AudioManager.LoadSound("donkeyKongHurt", "donkey-kong-hurt");
            AudioManager.LoadSong("jungleHijynx", "jungle-hijynx");
            AudioManager.PlaySong("jungleHijynx", true);
        }

        protected override bool IsLevelCompleted()
        {
            // Add your level completion logic here
            // For example, return true if player reaches a certain position
            return PlayerEntity.GetComponent<PositionComponent>().Position.X > 2000; // Adjust the completion criteria for level 2
        }
    }
}
