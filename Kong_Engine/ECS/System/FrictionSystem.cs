using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Kong_Engine.ECS.Component;

namespace Kong_Engine.ECS.System
{
    public class FrictionAndDragSystem
    {
        private readonly float _frictionCoefficient;
        private readonly float _dragCoefficient;

        public FrictionAndDragSystem(float frictionCoefficient, float dragCoefficient)
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

                    if (physicsComponent.IsGrounded)
                    {
                        physicsComponent.Velocity.X *= (1 - _frictionCoefficient * deltaTime);
                    }
                    else
                    {
                        physicsComponent.Velocity *= (1 - _dragCoefficient * deltaTime);
                    }
                }
            }
        }
    }
}
