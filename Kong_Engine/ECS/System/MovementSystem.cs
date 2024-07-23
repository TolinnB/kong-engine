using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.ECS.System
{
    public class MovementSystem
    {
        public void Update(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent<PositionComponent>() && entity.HasComponent<MovementComponent>())
                {
                    var positionComponent = entity.GetComponent<PositionComponent>();
                    var movementComponent = entity.GetComponent<MovementComponent>();

                    // Check boundaries and reverse direction if necessary
                    if (movementComponent.MovingRight)
                    {
                        positionComponent.Position += movementComponent.Velocity;

                        if (positionComponent.Position.X > movementComponent.RightBoundary)
                        {
                            movementComponent.MovingRight = false;
                            positionComponent.Position = new Vector2(movementComponent.RightBoundary, positionComponent.Position.Y);
                        }
                    }
                    else
                    {
                        positionComponent.Position -= movementComponent.Velocity;

                        if (positionComponent.Position.X < movementComponent.LeftBoundary)
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

