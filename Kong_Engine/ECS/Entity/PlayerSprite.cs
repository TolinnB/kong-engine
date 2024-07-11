using Kong_Engine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Kong_Engine.ECS.Component;
using Kong_Engine.ECS.Entity;

namespace Kong_Engine.Objects
{
    public class PlayerSprite : BaseEntity
    {
        public PlayerSprite(Texture2D texture)
        {
            AddComponent(new PositionComponent { Position = Vector2.Zero });
            AddComponent(new TextureComponent { Texture = texture });
        }

        public void MoveLeft()
        {
            var position = GetComponent<PositionComponent>();
            position.Position = new Vector2(position.Position.X - 5, position.Position.Y);
        }

        public void MoveRight()
        {
            var position = GetComponent<PositionComponent>();
            position.Position = new Vector2(position.Position.X + 5, position.Position.Y);
        }

        public void MoveDown()
        {
            var position = GetComponent<PositionComponent>();
            position.Position = new Vector2(position.Position.X, position.Position.Y + 5);
        }

        public void MoveUp()
        {
            var position = GetComponent<PositionComponent>();
            position.Position = new Vector2(position.Position.X, position.Position.Y - 5);
        }
    }
}
