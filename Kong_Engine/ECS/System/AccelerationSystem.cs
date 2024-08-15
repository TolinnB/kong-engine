using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

/**/
// Simple Acceletation System to manage an entity's velocity while moving
// Also handles gravity response and movement constraints
/**/

namespace Kong_Engine.ECS.System
{
    public class AccelerationSystem
    {
        public static void ApplyAcceleration(PhysicsComponent physicsComponent, float deltaTime, float gravity, float fallMultiplier, float verticalAcceleration, float horizontalAcceleration, float maxHorizontalSpeed, bool isMoving, bool isFacingRight)
        {
            // Apply vertical acceleration (gravity)
            var velocity = physicsComponent.Velocity; // Copy the Velocity to a local variable

            if (velocity.Y > 0)  // The entity is falling
            {
                velocity.Y += gravity * fallMultiplier * verticalAcceleration * deltaTime;
            }
            else if (velocity.Y < 0)  // The entity is moving upwards (jumping)
            {
                velocity.Y += gravity * verticalAcceleration * deltaTime;
            }

            // Apply horizontal acceleration for movement
            if (isMoving)
            {
                float desiredSpeed = isFacingRight ? maxHorizontalSpeed : -maxHorizontalSpeed;
                velocity.X = MathHelper.Clamp(velocity.X + horizontalAcceleration * deltaTime * Math.Sign(desiredSpeed - velocity.X), -maxHorizontalSpeed, maxHorizontalSpeed);
            }
            else
            {
                // Smooth deceleration when not moving
                velocity.X = MathHelper.Lerp(velocity.X, 0, 0.1f);
            }

            // Assign the modified velocity back to the PhysicsComponent
            physicsComponent.Velocity = velocity;
        }
    }
}
