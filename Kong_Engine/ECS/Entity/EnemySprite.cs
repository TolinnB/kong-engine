using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Kong_Engine.ECS.System;

namespace Kong_Engine.ECS.Entity
{
    public class EnemySprite : BaseEntity
    {
        public EnemySprite(Texture2D texture)
        {
            AddComponent(new PositionComponent { Position = new Vector2(1000, 100) });
            AddComponent(new TextureComponent { Texture = texture });
            AddComponent(new MovementComponent
            {
                Velocity = new Vector2(2, 0),
                LeftBoundary = 800,
                RightBoundary = 1200,
                MovingRight = true
            });
            AddComponent(new CollisionComponent
            {
                BoundingBox = new Rectangle(1000, 100, texture.Width, texture.Height)
            });
        }
    }
}
