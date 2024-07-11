using Kong_Engine.ECS.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.ECS.Entity
{
    public class EnemySprite : BaseEntity
    {
        public EnemySprite(Texture2D texture)
        {
            AddComponent(new PositionComponent { Position = new Vector2(1000, 100) }); // Initial position
            AddComponent(new TextureComponent { Texture = texture });
        }

        public void MoveTowardsPlayer(Vector2 playerPosition)
        {
            var position = GetComponent<PositionComponent>();
            Vector2 direction = Vector2.Normalize(playerPosition - position.Position);
            position.Position += direction * 2; // Move speed
        }
    }
}
