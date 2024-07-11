using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kong_Engine.ECS.System
{
    public class MovementSystem
    {
        public void Update(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent<PositionComponent>())
                {
                    var position = entity.GetComponent<PositionComponent>();
                    // Update position logic
                    position.Position += new Microsoft.Xna.Framework.Vector2(1, 0); // Example logic
                }
            }
        }
    }
}
