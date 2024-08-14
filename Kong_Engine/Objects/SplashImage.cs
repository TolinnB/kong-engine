using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class SplashImage : BaseGameObject
    {
        private float _alpha = 0f;  // Alpha value for fade-in, starts at 0 (fully transparent)
        private float _fadeSpeed = 0.5f;  // Speed at which the image fades in

        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            // Increase alpha value to create fade-in effect
            _alpha += _fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Clamp the alpha value to 1 (fully opaque)
            _alpha = MathHelper.Clamp(_alpha, 0f, 1f);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // Render the image with the current alpha value
            spriteBatch.Draw(_texture, _position, Color.White * _alpha);
        }

        public bool IsFullyVisible()
        {
            return _alpha >= 1f;
        }
    }
}
