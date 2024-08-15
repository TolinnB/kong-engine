using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class SplashImage : BaseGameObject
    {
        private float _alpha = 0f;  // Alpha value for fade-in
        private float _fadeSpeed = 0.5f; // Fade in speed

        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            _alpha += _fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _alpha = MathHelper.Clamp(_alpha, 0f, 1f);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White * _alpha);
        }

        public bool IsFullyVisible()
        {
            return _alpha >= 1f;
        }
    }
}