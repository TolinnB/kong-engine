using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.System;
using Kong_Engine.Objects;
using Microsoft.Xna.Framework;
using Moq;
using System.Collections.Generic;

namespace KongEngineTests.ECS.System
{
    [TestClass]
    public class CollisionSystemTests
    {
        [TestMethod]
        public void Update_ShouldDetectCollision()
        {
            var audioManagerMock = new Mock<AudioManager>(null);
            var collisionSystem = new CollisionSystem(audioManagerMock.Object);

            var playerEntity = new PlayerSprite(null);
            var enemyEntity = new EnemySprite(null);

            var playerPosition = new PositionComponent { Position = new Vector2(0, 0) };
            var playerCollision = new CollisionComponent { BoundingBox = new Rectangle(0, 0, 10, 10) };
            var playerLife = new LifeComponent { Lives = 10 };

            var enemyPosition = new PositionComponent { Position = new Vector2(5, 5) };
            var enemyCollision = new CollisionComponent { BoundingBox = new Rectangle(5, 5, 10, 10) };

            playerEntity.AddComponent(playerPosition);
            playerEntity.AddComponent(playerCollision);
            playerEntity.AddComponent(playerLife);
            enemyEntity.AddComponent(enemyPosition);
            enemyEntity.AddComponent(enemyCollision);

            var entities = new List<BaseEntity> { playerEntity, enemyEntity };

            collisionSystem.Update(entities);

            // Assert that collision was handled (player should have lost a life)
            Assert.AreEqual(9, playerLife.Lives);
        }
    }
}
