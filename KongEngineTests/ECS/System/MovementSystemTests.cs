using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Kong_Engine.ECS.System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KongEngineTests.ECS.System
{
    [TestClass]
    public class MovementSystemTests
    {
        [TestMethod]
        public void Update_ShouldMoveEntityWithinBoundaries()
        {
            var entity = new BaseEntity();
            var positionComponent = new PositionComponent { Position = new Vector2(0, 0) };
            var movementComponent = new MovementComponent
            {
                Velocity = new Vector2(1, 0),
                LeftBoundary = 0,
                RightBoundary = 10,
                MovingRight = true
            };

            entity.AddComponent(positionComponent);
            entity.AddComponent(movementComponent);

            var entities = new List<BaseEntity> { entity };
            var movementSystem = new MovementSystem();

            movementSystem.Update(entities);

            Assert.AreEqual(new Vector2(1, 0), positionComponent.Position);
        }

        [TestMethod]
        public void Update_ShouldReverseDirectionAtBoundaries()
        {
            var entity = new BaseEntity();
            var positionComponent = new PositionComponent { Position = new Vector2(10, 0) };
            var movementComponent = new MovementComponent
            {
                Velocity = new Vector2(1, 0),
                LeftBoundary = 0,
                RightBoundary = 10,
                MovingRight = true
            };

            entity.AddComponent(positionComponent);
            entity.AddComponent(movementComponent);

            var entities = new List<BaseEntity> { entity };
            var movementSystem = new MovementSystem();

            movementSystem.Update(entities);

            Assert.AreEqual(new Vector2(10, 0), positionComponent.Position);
            Assert.IsFalse(movementComponent.MovingRight);
        }
    }
}
