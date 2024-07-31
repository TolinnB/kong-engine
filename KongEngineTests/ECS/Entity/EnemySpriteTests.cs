using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.ECS.Component;
using Kong_Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;

namespace KongEngineTests.ECS.Entity
{
    [TestClass]
    public class EnemySpriteTests
    {
        [TestMethod]
        public void Update_ShouldMoveWithinBoundaries()
        {
            var textureMock = new Mock<Texture2D>();
            var enemy = new EnemySprite(textureMock.Object);
            var positionComponent = new PositionComponent { Position = new Vector2(900, 100) };
            var movementComponent = new MovementComponent
            {
                Velocity = new Vector2(1, 0),
                LeftBoundary = 800,
                RightBoundary = 1200,
                MovingRight = true
            };

            enemy.AddComponent(positionComponent);
            enemy.AddComponent(movementComponent);

            var gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromMilliseconds(16));

            enemy.Update(gameTime);

            Assert.AreEqual(new Vector2(901, 100), positionComponent.Position);
        }
    }
}
