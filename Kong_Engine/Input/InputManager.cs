using System;
using System.Collections.Generic;
using Kong_Engine.Input.Base;
using Microsoft.Xna.Framework;
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
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            var mouseState = Mouse.GetState();

            var keyboardCommands = _inputMapper.GetKeyboardState(keyboardState);
            var gamePadCommands = _inputMapper.GetGamePadState(gamePadState);
            var mouseCommands = _inputMapper.GetMouseState(mouseState);

            foreach (var command in keyboardCommands)
            {
                handleCommand(command);
            }

            foreach (var command in gamePadCommands)
            {
                handleCommand(command);
            }

            foreach (var command in mouseCommands)
            {
                handleCommand(command);
            }
        }
    }
}
