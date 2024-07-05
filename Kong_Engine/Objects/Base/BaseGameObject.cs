using Kong_Engine.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        public Vector2 Position { get; set; }

        public virtual int zIndex { get; set; }

        public virtual void Update(GameTime gameTime)
        {
           //Implement later
        }

        public virtual void OnNotify(Events eventType) { }

        public void Render(SpriteBatch spriteBatch)
        {
            // TODO: Drawing call here
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
