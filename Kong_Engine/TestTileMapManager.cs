using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using TiledSharp;

namespace Kong_Engine.Tests
{
    public class TestTileMapManager : TileMapManager
    {
        public TestTileMapManager()
            : base(
                new Mock<ContentManager>().Object,
                new Mock<SpriteBatch>(new Mock<GraphicsDevice>().Object).Object,
                new Mock<TmxMap>().Object,
                new Mock<Texture2D>(new Mock<GraphicsDevice>().Object, 1, 1).Object,
                1, 32, 32, 1.0f)
        {
            // You can override methods or properties here if needed for testing
        }
    }
}
