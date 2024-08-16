using Kong_Engine.Objects;
using Kong_Engine.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Xunit;
using System.Reflection;

namespace Kong_Engine.Tests
{
    public class BaseLevelStateTests
    {
        private class TestLevelState : BaseLevelState
        {
            protected override void LoadLevelContent() { /* no-op for test */ }
            protected override void InitializeEntities() { /* no-op for test */ }
            protected override bool IsLevelCompleted() => true;
            protected override void SetInputManager() { /* no-op for test */ }
        }

        [Fact]
        public void Initialize_ShouldInitializeSystems()
        {
            // Arrange
            var mockContentManager = new Mock<ContentManager>(MockBehavior.Strict);
            var mockMainGame = new Mock<MainGame>();
            var testLevelState = new TestLevelState();

            // Act
            testLevelState.Initialize(mockContentManager.Object, mockMainGame.Object);

            // Assert using reflection
            var movementSystem = typeof(BaseLevelState).GetField("MovementSystem", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(testLevelState);
            var audioManager = typeof(BaseLevelState).GetField("AudioManager", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(testLevelState);
            var spriteBatch = typeof(BaseLevelState).GetField("SpriteBatch", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(testLevelState);

            Assert.NotNull(movementSystem);
            Assert.NotNull(audioManager);
            Assert.NotNull(spriteBatch);
        }

        [Fact]
        public void Update_ShouldUpdateEntitiesAndSystems()
        {
            // Arrange
            var mockContentManager = new Mock<ContentManager>(MockBehavior.Strict);
            var mockMainGame = new Mock<MainGame>();
            var mockPlayer = new Mock<PlayerSprite>();
            var mockEnemy = new Mock<EnemySprite>();
            var mockGameTime = new Mock<GameTime>();
            var testLevelState = new TestLevelState();
            testLevelState.Initialize(mockContentManager.Object, mockMainGame.Object);

            // Using reflection to set protected properties
            var playerEntityField = typeof(BaseLevelState).GetField("PlayerEntity", BindingFlags.NonPublic | BindingFlags.Instance);
            var enemyEntityField = typeof(BaseLevelState).GetField("EnemyEntity", BindingFlags.NonPublic | BindingFlags.Instance);
            playerEntityField?.SetValue(testLevelState, mockPlayer.Object);
            enemyEntityField?.SetValue(testLevelState, mockEnemy.Object);

            // Act
            testLevelState.Update(mockGameTime.Object);

            // Assert
            mockPlayer.Verify(p => p.Update(It.IsAny<GameTime>()), Times.Once);
            mockEnemy.Verify(e => e.Update(It.IsAny<GameTime>()), Times.Once);
        }

        [Fact]
        public void Render_ShouldDrawEntities()
        {
            // Arrange
            var mockSpriteBatch = new Mock<SpriteBatch>(MockBehavior.Strict);
            var mockContentManager = new Mock<ContentManager>(MockBehavior.Strict);
            var mockMainGame = new Mock<MainGame>();
            var mockPlayer = new Mock<PlayerSprite>();
            var mockEnemy = new Mock<EnemySprite>();
            var testLevelState = new TestLevelState();
            testLevelState.Initialize(mockContentManager.Object, mockMainGame.Object);

            // Using reflection to set protected properties
            var playerEntityField = typeof(BaseLevelState).GetField("PlayerEntity", BindingFlags.NonPublic | BindingFlags.Instance);
            var enemyEntityField = typeof(BaseLevelState).GetField("EnemyEntity", BindingFlags.NonPublic | BindingFlags.Instance);
            playerEntityField?.SetValue(testLevelState, mockPlayer.Object);
            enemyEntityField?.SetValue(testLevelState, mockEnemy.Object);

            // Act
            testLevelState.Render(mockSpriteBatch.Object);

            // Assert
            mockPlayer.Verify(p => p.Draw(It.IsAny<SpriteBatch>(), It.IsAny<Matrix>()), Times.Once);
            mockEnemy.Verify(e => e.Draw(It.IsAny<SpriteBatch>(), It.IsAny<Matrix>()), Times.Once);
        }
    }
}
