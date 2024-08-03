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
                    var velocity = physicsComponent.Velocity;
                    var positionComponent = entity.GetComponent<PositionComponent>();
                    var position = positionComponent.Position;

                    if (keyboardState.IsKeyDown(Keys.Space) && physicsComponent.IsGrounded)
                    {
                        velocity.Y = -_jumpSpeed;
                        physicsComponent.IsGrounded = false;
                    }

                    if (!physicsComponent.IsGrounded)
                    {
                        velocity.Y += _gravity * deltaTime;
                        position += velocity * deltaTime;

                        // Check if the entity has landed
                        if (position.Y >= 100) // Assuming ground level is y=100
                        {
                            position.Y = 100;
                            physicsComponent.IsGrounded = true;
                            velocity.Y = 0f;
                        }
                    }

                    // Assign the modified values back to the components
                    physicsComponent.Velocity = velocity;
                    positionComponent.Position = position;
                }
            }
        }
    }
}
