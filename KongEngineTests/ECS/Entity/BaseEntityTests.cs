using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Microsoft.Xna.Framework;

namespace KongEngineTests.ECS.Entity
{
    [TestClass]
    public class BaseEntityTests
    {
        [TestMethod]
        public void AddComponent_ShouldAddComponent()
        {
            var entity = new BaseEntity();
            var positionComponent = new PositionComponent { Position = new Vector2(100, 100) };

            entity.AddComponent(positionComponent);

            Assert.IsTrue(entity.HasComponent<PositionComponent>());
            Assert.AreEqual(positionComponent, entity.GetComponent<PositionComponent>());
        }

        [TestMethod]
        public void RemoveComponent_ShouldRemoveComponent()
        {
            var entity = new BaseEntity();
            var positionComponent = new PositionComponent { Position = new Vector2(100, 100) };

            entity.AddComponent(positionComponent);
            entity.RemoveComponent<PositionComponent>();

            Assert.IsFalse(entity.HasComponent<PositionComponent>());
        }
    }
}
