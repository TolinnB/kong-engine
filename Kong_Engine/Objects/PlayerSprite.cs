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
            // Adjust the X coordinate to move left
            Position = new Vector2(Position.X - 5, Position.Y); // Adjust the value 5 as needed
        }

        // Method to move the player right
        public void MoveRight()
        {
            // Adjust the X coordinate to move right
            Position = new Vector2(Position.X + 5, Position.Y); // Adjust the value 5 as needed
        }

        // Override the Update method to include custom behavior if needed
        public override void Update(GameTime gameTime)
        {
            // Custom update logic
            base.Update(gameTime);
        }
    }
}