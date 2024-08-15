using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Xml.Linq;

/**/
// Simple Gravity System
// Applies downward force to an object until it collides with the floor
/**/

namespace Kong_Engine.ECS.System
{
    public class GravitySystem
{
    private readonly Vector2 _gravity;

    public GravitySystem(Vector2 gravity)
    {
        _gravity = gravity;
    }

    public void Update(IEnumerable<BaseEntity> entities, float deltaTime)
    {
        foreach (var entity in entities)
        {
            if (entity.HasComponent<PhysicsComponent>())
            {
                var physicsComponent = entity.GetComponent<PhysicsComponent>();
                if (!physicsComponent.IsGrounded)
                {
                    physicsComponent.Velocity += _gravity * deltaTime;
                }
            }
        }
    }
}
}

