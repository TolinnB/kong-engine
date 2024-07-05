using Kong_Engine.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Kong_Engine.Objects
{
    public class PlayerSprite : BaseGameObject
    {
        public PlayerSprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void MoveLeft()
        {
            
            Position = new Vector2(Position.X - 5, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + 5, Position.Y);
        }

        public void MoveDown()
        {
            Position = new Vector2(Position.X, Position.Y + 5);
        }

        public void MoveUp()
        {
            Position = new Vector2(Position.X, Position.Y - 5);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}