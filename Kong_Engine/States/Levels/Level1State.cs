using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
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
    }
}
