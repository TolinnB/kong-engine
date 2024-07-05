using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kong_Engine.Input.Base
{
    public abstract class BaseInputMapper
    {
        public abstract IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state);
        // Gamepad support will go here
    }
}
