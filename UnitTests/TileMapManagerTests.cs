using Xunit;

namespace Kong_Engine.Tests
{
    public class TileMapManagerTests
    {
        [Fact]
        public void SetScale_ShouldUpdateScale()
        {
            // Arrange
            var tileMapManager = new TestTileMapManager();

            float newScale = 2.0f;

            // Act
            tileMapManager.SetScale(newScale);

            // Assert
            Assert.Equal(newScale, tileMapManager.ScaleFactor);
        }
    }
}
