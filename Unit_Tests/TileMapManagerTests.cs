using Kong_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using TiledSharp;
using Xunit;

namespace Unit_Tests
{
    public class TileMapManagerTests
    {
        private GraphicsDevice CreateGraphicsDevice()
        {
            var gdm = new GraphicsDeviceManager(new Game());
            gdm.CreateDevice();
            return gdm.GraphicsDevice;
        }

        [Fact]
        public void TileMapManager_Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var contentManager = new Mock<ContentManager>();
            var graphicsDevice = CreateGraphicsDevice();
            var spriteBatch = new Mock<SpriteBatch>(graphicsDevice);
            var map = new TmxMap("dummy_map.tmx");
            var tileset = new Mock<Texture2D>(MockBehavior.Loose, graphicsDevice);
            int tileWidth = 32;
            int tileHeight = 32;

            // Act
            var tileMapManager = new TileMapManager(contentManager.Object, spriteBatch.Object, map, tileset.Object, 10, tileWidth, tileHeight);

            // Assert
            Assert.Equal(tileWidth, tileMapManager.TileWidth);
            Assert.Equal(map, tileMapManager.Map);
            Assert.NotNull(tileMapManager.CollisionRectangles);
            Assert.Empty(tileMapManager.CollisionRectangles);
        }

        [Fact]
        public void TileMapManager_SetScale_ShouldUpdateScaleAndReloadCollisions()
        {
            // Arrange
            var contentManager = new Mock<ContentManager>();
            var graphicsDevice = CreateGraphicsDevice();
            var spriteBatch = new Mock<SpriteBatch>(graphicsDevice);
            var map = new TmxMap("dummy_map.tmx");
            var tileset = new Mock<Texture2D>(MockBehavior.Loose, graphicsDevice);
            var tileMapManager = new TileMapManager(contentManager.Object, spriteBatch.Object, map, tileset.Object, 10, 32, 32);

            // Act
            tileMapManager.SetScale(1.5f);

            // Assert
            Assert.Equal(1.5f, tileMapManager.Scale);
            Assert.NotEmpty(tileMapManager.CollisionRectangles);
        }

        [Fact]
        public void TileMapManager_LoadBackgrounds_ShouldLoadAllBackgroundTextures()
        {
            // Arrange
            var contentManager = new Mock<ContentManager>();
            contentManager.Setup(c => c.Load<Texture2D>(It.IsAny<string>())).Returns(new Mock<Texture2D>(MockBehavior.Loose, CreateGraphicsDevice()).Object);
            var spriteBatch = new Mock<SpriteBatch>(CreateGraphicsDevice());
            var map = new TmxMap("dummy_map.tmx");
            var tileset = new Mock<Texture2D>(MockBehavior.Loose, CreateGraphicsDevice());
            var tileMapManager = new TileMapManager(contentManager.Object, spriteBatch.Object, map, tileset.Object, 10, 32, 32);

            // Act
            var backgrounds = tileMapManager.BackgroundTextures;

            // Assert
            Assert.NotNull(backgrounds);
            Assert.Equal(3, backgrounds.Count);
        }

        [Fact]
        public void TileMapManager_Draw_ShouldRenderCorrectly()
        {
            // Arrange
            var contentManager = new Mock<ContentManager>();
            var graphicsDevice = CreateGraphicsDevice();
            var spriteBatch = new Mock<SpriteBatch>(graphicsDevice);
            var map = new TmxMap("dummy_map.tmx");
            var tileset = new Mock<Texture2D>(MockBehavior.Loose, graphicsDevice);
            var tileMapManager = new TileMapManager(contentManager.Object, spriteBatch.Object, map, tileset.Object, 10, 32, 32);
            Matrix matrix = Matrix.Identity;

            // Act
            tileMapManager.Draw(matrix);

            // Assert
            spriteBatch.Verify(s => s.Begin(It.IsAny<SpriteSortMode>(), null, It.IsAny<SamplerState>(), null, null, null, matrix), Times.Once);
            spriteBatch.Verify(s => s.End(), Times.Once);
        }

        [Fact]
        public void TileMapManager_DrawCollisionRectangles_ShouldRenderCollisions()
        {
            // Arrange
            var contentManager = new Mock<ContentManager>();
            var graphicsDevice = CreateGraphicsDevice();
            var spriteBatch = new Mock<SpriteBatch>(graphicsDevice);
            var map = new TmxMap("dummy_map.tmx");
            var tileset = new Mock<Texture2D>(MockBehavior.Loose, graphicsDevice);
            var tileMapManager = new TileMapManager(contentManager.Object, spriteBatch.Object, map, tileset.Object, 10, 32, 32);

            // Simulate some collision rectangles
            tileMapManager.CollisionRectangles.Add(new Rectangle(0, 0, 32, 32));
            tileMapManager.CollisionRectangles.Add(new Rectangle(32, 32, 32, 32));

            // Act
            tileMapManager.DrawCollisionRectangles(spriteBatch.Object);

            // Assert
            spriteBatch.Verify(s => s.Draw(It.IsAny<Texture2D>(), It.IsAny<Rectangle>(), It.IsAny<Color>()), Times.Exactly(2));
        }
    }
}

