using Kong_Engine;
using Kong_Engine.States.Base;
using Moq;
using Xunit;

namespace Unit_Tests
{
    public class MainGameTests
    {
        [Fact]
        public void MainGame_Initialize_ShouldSetInitialValues()
        {
            // Arrange
            var game = new MainGame();

            // Act
            game.InitializeGame(); // Use the public method for testing

            // Assert
            Assert.NotNull(game.AudioManager);
            Assert.True(game.IsMouseVisible);
            Assert.Equal(1280, game.GraphicsManager.PreferredBackBufferWidth);
            Assert.Equal(720, game.GraphicsManager.PreferredBackBufferHeight);
        }

        [Fact]
        public void MainGame_SwitchState_ShouldUpdateCurrentState()
        {
            // Arrange
            var game = new MainGame();
            var mockState = new Mock<BaseGameState>();

            // Act
            game.SwitchState(mockState.Object);

            // Assert
            Assert.Equal(mockState.Object, game.CurrentState);
            mockState.Verify(s => s.Initialize(It.IsAny<Microsoft.Xna.Framework.Content.ContentManager>(), game), Times.Once);
            mockState.Verify(s => s.LoadContent(), Times.Once);
        }
    }
}