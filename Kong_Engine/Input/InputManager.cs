using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kong_Engine.Input.Base;
using Microsoft.Xna.Framework.Input;

namespace Kong_Engine.Input
{
    public class InputManager
    {
        private readonly BaseInputMapper _inputMapper;

        public InputManager(BaseInputMapper inputMapper)
        {
            _inputMapper = inputMapper;
        }

        public void GetCommands(Action<BaseInputCommand> handleCommand)
        {
            var keyboardState = Keyboard.GetState();
            var commands = _inputMapper.GetKeyboardState(keyboardState);

            foreach (var command in commands)
            {
                handleCommand(command);
            }
        }
    }
}
