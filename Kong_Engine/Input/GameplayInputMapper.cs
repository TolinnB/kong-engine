using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Kong_Engine.Input;
using Kong_Engine.Input.Base;  // Ensure this using directive is included

namespace Kong_Engine.Input
{
    public class GameplayInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<BaseInputCommand>();

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }
            if (state.IsKeyDown(Keys.Left))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveLeft());
            }
            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }
            if (state.IsKeyDown(Keys.Down))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveDown());
            }
            if (state.IsKeyDown(Keys.Up))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveUp());
            }

            return commands;
        }

        public override IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state)
        {
            var commands = new List<BaseInputCommand>();

            if (state.IsButtonDown(Buttons.Back))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }
            if (state.ThumbSticks.Left.X < -0.5f || state.DPad.Left == ButtonState.Pressed)
            {
                commands.Add(new GameplayInputCommand.PlayerMoveLeft());
            }
            if (state.ThumbSticks.Left.X > 0.5f || state.DPad.Right == ButtonState.Pressed)
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }
            if (state.ThumbSticks.Left.Y < -0.5f || state.DPad.Down == ButtonState.Pressed)
            {
                commands.Add(new GameplayInputCommand.PlayerMoveDown());
            }
            if (state.ThumbSticks.Left.Y > 0.5f || state.DPad.Up == ButtonState.Pressed)
            {
                commands.Add(new GameplayInputCommand.PlayerMoveUp());
            }

            return commands;
        }
    }
}