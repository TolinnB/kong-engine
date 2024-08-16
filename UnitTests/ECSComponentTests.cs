using Kong_Engine.ECS.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using Xunit;

namespace Kong_Engine.Tests
{
    public class ECSComponentTests
    {
        [Fact]
        public void PositionComponent_ShouldInitializeCorrectly()
        {
            var positionComponent = new PositionComponent { Position = new Vector2(100, 200) };
            Assert.Equal(new Vector2(100, 200), positionComponent.Position);
        }

        [Fact]
        public void TextureComponent_ShouldInitializeCorrectly()
        {
            // Substitute for Texture2D creation
            var graphicsDevice = new Mock<GraphicsDevice>();
            var texture = new Texture2D(graphicsDevice.Object, 100, 100);
            var textureComponent = new TextureComponent { Texture = texture };

            Assert.Equal(texture, textureComponent.Texture);
        }

        [Fact]
        public void MovementComponent_ShouldInitializeCorrectly()
        {
            var movementComponent = new MovementComponent
            {
                Velocity = new Vector2(10, 15),
                LeftBoundary = 0,
                RightBoundary = 1000,
                MovingRight = true
            };

            Assert.Equal(new Vector2(10, 15), movementComponent.Velocity);
            Assert.Equal(0, movementComponent.LeftBoundary);
            Assert.Equal(1000, movementComponent.RightBoundary);
            Assert.True(movementComponent.MovingRight);
        }

        [Fact]
        public void CollisionComponent_ShouldInitializeCorrectly()
        {
            var collisionComponent = new CollisionComponent { BoundingBox = new Rectangle(10, 20, 30, 40) };
            Assert.Equal(new Rectangle(10, 20, 30, 40), collisionComponent.BoundingBox);
        }

        [Fact]
        public void LifeComponent_ShouldInitializeCorrectly()
        {
            var lifeComponent = new LifeComponent { Lives = 3 };
            Assert.Equal(3, lifeComponent.Lives);
        }

        [Fact]
        public void PhysicsComponent_ShouldInitializeCorrectly()
        {
            var physicsComponent = new PhysicsComponent
            {
                Velocity = new Vector2(5, 10),
                Mass = 1.5f,
                IsGrounded = true
            };

            Assert.Equal(new Vector2(5, 10), physicsComponent.Velocity);
            Assert.Equal(1.5f, physicsComponent.Mass);
            Assert.True(physicsComponent.IsGrounded);
        }
    }
}
