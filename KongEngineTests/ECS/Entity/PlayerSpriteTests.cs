using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.ECS.Component;
using Kong_Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using System;

namespace KongEngineTests.ECS.Entity
{
    [TestClass]
    public class PlayerSpriteTests
    {
        [TestMethod]
        public void Update_ShouldHandleJumping()
        {
            var textureMock = new Mock<Texture2D>();
            var player = new PlayerSprite(textureMock.Object);
            var positionComponent = new PositionComponent { Position = new Vector2(0, 100) };
            player.AddComponent(positionComponent);

            var gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromMilliseconds(16));

            player.Update(gameTime);

            Assert.AreEqual(100, positionComponent.Position.Y);
        }
    }
}
