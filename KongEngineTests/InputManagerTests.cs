using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine.Input;
using Kong_Engine.Input.Base;
using Microsoft.Xna.Framework.Input;
using Moq;

namespace KongEngineTests.Input
{
    [TestClass]
    public class InputManagerTests
    {
        [TestMethod]
        public void GetCommands_ShouldHandleKeyboardInput()
        {
            var inputMapper = new GameplayInputMapper();
            var inputManager = new InputManager(inputMapper);

            bool commandHandled = false;

            inputManager.GetCommands(command =>
            {
                if (command is GameplayInputCommand.PlayerMoveLeft)
                {
                    commandHandled = true;
                }
            });

            Assert.IsTrue(commandHandled);
        }
    }
}
