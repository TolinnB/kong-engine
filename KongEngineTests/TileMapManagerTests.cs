using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using TiledSharp;

namespace KongEngineTests
{
    [TestClass]
    public class TileMapManagerTests
    {
        [TestMethod]
        public void SetScale_ShouldUpdateScale()
        {
            var spriteBatchMock = new Mock<SpriteBatch>();
            var mapMock = new Mock<TmxMap>();
            var textureMock = new Mock<Texture2D>();

            var tileMapManager = new TileMapManager(spriteBatchMock.Object, mapMock.Object, textureMock.Object, 1, 32, 32, 1.0f);

            tileMapManager.SetScale(2.0f);

            // Verify that the scale has been updated (could be checked in a more detailed way if scale was accessible)
            var scaleField = tileMapManager.GetType().GetField("scale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.AreEqual(2.0f, (float)scaleField.GetValue(tileMapManager));
        }
    }
}
