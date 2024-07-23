using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class TerrainBackground : BaseGameObject
    {
        public TerrainBackground(Texture2D texture)
        {
            _texture = texture;
            _position = Vector2.Zero;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            int tilesX = viewport.Width / _texture.Width + 2;
            int tilesY = viewport.Height / _texture.Height + 1;

            for (int y = 0; y < tilesY; y++)
            {
                for (int x = 0; x < tilesX; x++)
                {
                    var destinationRectangle = new Rectangle(
                        (int)_position.X + x * _texture.Width,
                        y * _texture.Height,
                        _texture.Width,
                        _texture.Height);

                    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        public void UpdateBackgroundPosition(Vector2 playerPosition)
        {
            _position.X = -playerPosition.X % _texture.Width;

            if (_position.X > 0)
            {
                _position.X -= _texture.Width;
            }
        }
    }

}