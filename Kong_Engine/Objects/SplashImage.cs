using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Kong_Engine.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}