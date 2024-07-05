using Kong_Engine.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;

        private Vector2 _position;

        public int zIndex;

        public virtual void OnNotify(Events eventType) { }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Vector2.One, Color.White);
        }
    }
}