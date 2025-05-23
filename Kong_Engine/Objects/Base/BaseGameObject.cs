using Kong_Engine.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/**/
// Base structure for game objects
// You can change height, width, behaviour and rendering here from derived objects
/**/

namespace Kong_Engine.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        protected Vector2 _position = Vector2.One;

        public int zIndex;

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void OnNotify(Events eventType, object argument = null) { }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}