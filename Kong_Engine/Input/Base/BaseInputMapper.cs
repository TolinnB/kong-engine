using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**/
// Base class to initialise inputs for your game
// Subclasses of BaseInputMapper can interpret how input is handled
/**/

namespace Kong_Engine.Input.Base
{
    public abstract class BaseInputMapper
    {
        public abstract IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state);
        public abstract IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state);
    }
}
