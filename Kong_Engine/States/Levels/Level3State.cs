
using Kong_Engine.ECS.Component;
using Kong_Engine.Input;
using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Kong_Engine.States.Levels
{
    public class Level3State : BaseLevelState
    {
        protected override void LoadLevelContent()
        {
            var tilesetBase = Content.Load<Texture2D>("QwestQuest/Grass"); // Update the texture name if necessary
            var tilesetDirt = Content.Load<Texture2D>("QwestQuest/Tilled_Dirt");
            var tilesetWater = Content.Load<Texture2D>("QwestQuest/Water");
            var tilesetBridges= Content.Load<Texture2D>("QwestQuest/Wooden_House_Walls_Tilset");

            var tilesetCollision = Content.Load<Texture2D>("QwestQuest/collisions");
            
            var map = new TmxMap("Content/QwestQuest/QuestQuestMap.tmx"); // Assuming the third level's Tiled map file
            int tilesetTilesWide = tilesetBase.Width / map.Tilesets[0].TileWidth;
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            float scale = 2.0f;

            TileMapManager = new TileMapManager(SpriteBatch, map, tilesetBase, tilesetTilesWide, tileWidth, tileHeight, scale);
        }

        protected override void InitializeEntities()
        {
            var playerSpriteSheet = Content.Load<Texture2D>("RACCOONSPRITESHEET");
            

            PlayerEntity = new PlayerSprite(playerSpriteSheet);
            

            Entities.Add(PlayerEntity);
            
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
            return PlayerEntity.GetComponent<PositionComponent>().Position.X > 3000; // Adjust the completion criteria for level 3
        }
    }
}
