using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Kong_Engine.ECS.Component;

/**/
// Simple Bouyancy physics
// Entities in water will be pushed upwards dependant on their weight and depth
/**/

namespace Kong_Engine.ECS.System
{
    public class BuoyancySystem
    {
        private readonly float _waterLevel;
        private readonly float _buoyancyFactor;

        public BuoyancySystem(float waterLevel, float buoyancyFactor)
        {
            _waterLevel = waterLevel;
            _buoyancyFactor = buoyancyFactor;
        }

        public void Update(IEnumerable<BaseEntity> entities, float deltaTime)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent<PhysicsComponent>())
                {
                    var physicsComponent = entity.GetComponent<PhysicsComponent>();
                    var positionComponent = entity.GetComponent<PositionComponent>();

                    if (positionComponent.Position.Y > _waterLevel)
                    {
                        float displacement = _waterLevel - positionComponent.Position.Y;
                        Vector2 buoyancyForce = new Vector2(0, -displacement * _buoyancyFactor * physicsComponent.Mass);
                        physicsComponent.Velocity += buoyancyForce * deltaTime;
                    }
                }
            }
        }
    }
}
