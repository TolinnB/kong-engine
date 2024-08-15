using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

/**/
// Simple Movement System 
// Handles Horizontal movement for entities
// changes with directional input, velocity, and keeps them in the bounds of the screen
/**/

namespace Kong_Engine.ECS.System
{
    public class MovementSystem
    {
        private int screenWidth;

        public MovementSystem(int screenWidth)
        {
            this.screenWidth = screenWidth;
        }

        public void Update(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity == null)
                {
                    continue; // Skip null entities
                }

                if (entity.HasComponent<PositionComponent>() && entity.HasComponent<MovementComponent>())
                {
                    var positionComponent = entity.GetComponent<PositionComponent>();
                    var movementComponent = entity.GetComponent<MovementComponent>();

                    // Check boundaries and reverse direction if necessary
                    if (movementComponent.MovingRight)
                    {
                        positionComponent.Position += movementComponent.Velocity;

                        if (positionComponent.Position.X > movementComponent.RightBoundary ||
                            positionComponent.Position.X + movementComponent.Velocity.X > screenWidth)
                        {
                            movementComponent.MovingRight = false;
                            positionComponent.Position = new Vector2(movementComponent.RightBoundary, positionComponent.Position.Y);
                        }
                    }
                    else
                    {
                        positionComponent.Position -= movementComponent.Velocity;

                        if (positionComponent.Position.X < movementComponent.LeftBoundary ||
                            positionComponent.Position.X - movementComponent.Velocity.X < 0)
                        {
                            movementComponent.MovingRight = true;
                            positionComponent.Position = new Vector2(movementComponent.LeftBoundary, positionComponent.Position.Y);
                        }
                    }
                }
            }
        }
    }
}
