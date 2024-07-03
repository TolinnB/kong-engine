using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kong_Engine.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace Kong_Engine.Objects
{
    public class MainMenuImage : BaseGameObject
    {
        public MainMenuImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
