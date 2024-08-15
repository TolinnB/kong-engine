using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Kong_Engine.ECS.Component;

/**/
// Simple Friction System
// Applies drag and friction. These can be manipulated to affect running and air resistance
// with gradual slow down
/**/

namespace Kong_Engine.ECS.System
{
    public class FrictionSystem
    {
        private readonly float _frictionCoefficient;
        private readonly float _dragCoefficient;

        public FrictionSystem(float frictionCoefficient, float dragCoefficient)
        {
            _frictionCoefficient = frictionCoefficient;
            _dragCoefficient = dragCoefficient;
        }

        public void Update(IEnumerable<BaseEntity> entities, float deltaTime)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent<PhysicsComponent>())
                {
                    var physicsComponent = entity.GetComponent<PhysicsComponent>();
                    var velocity = physicsComponent.Velocity;

                    if (physicsComponent.IsGrounded)
                    {
                        velocity.X *= (1 - _frictionCoefficient * deltaTime);
                    }
                    else
                    {
                        velocity *= (1 - _dragCoefficient * deltaTime);
                    }

                    physicsComponent.Velocity = velocity;
                }
            }
        }
    }
}
