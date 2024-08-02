using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.Objects;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.System
{
    public class JumpingSystem
    {
        private readonly float _jumpSpeed;
        private readonly float _gravity;

        public JumpingSystem(float jumpSpeed, float gravity)
        {
            _jumpSpeed = jumpSpeed;
            _gravity = gravity;
        }

        public void Update(IEnumerable<BaseEntity> entities, float deltaTime)
        {
            var keyboardState = Keyboard.GetState();

            foreach (var entity in entities)
            {
                if (entity.HasComponent<PhysicsComponent>())
                {
                    var physicsComponent = entity.GetComponent<PhysicsComponent>();

                    if (keyboardState.IsKeyDown(Keys.Space) && physicsComponent.IsGrounded)
                    {
                        physicsComponent.Velocity.Y = -_jumpSpeed;
                        physicsComponent.IsGrounded = false;
                    }

                    if (!physicsComponent.IsGrounded)
                    {
                        physicsComponent.Velocity.Y += _gravity * deltaTime;
                        var positionComponent = entity.GetComponent<PositionComponent>();
                        positionComponent.Position += physicsComponent.Velocity * deltaTime;

                        // Check if the entity has landed
                        if (positionComponent.Position.Y >= 100) // Assuming ground level is y=100
                        {
                            positionComponent.Position.Y = 100;
                            physicsComponent.IsGrounded = true;
                            physicsComponent.Velocity.Y = 0f;
                        }
                    }
                }
            }
        }
    }
}
