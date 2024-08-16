using System;
using System.Collections.Generic;
using Kong_Engine.Input;
using Kong_Engine.Input.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using Xunit;

namespace Kong_Engine.Tests
{
    public class InputManagerTests
    {
        [Fact]
        public void GetCommands_ShouldInvokeHandleCommandForEachKeyboardCommand()
        {
            // Arrange
            var mockInputMapper = new Mock<BaseInputMapper>();
            var mockKeyboardCommands = new List<BaseInputCommand> { new Mock<BaseInputCommand>().Object };
            var mockGamePadCommands = new List<BaseInputCommand>();

            mockInputMapper.Setup(im => im.GetKeyboardState(It.IsAny<KeyboardState>())).Returns(mockKeyboardCommands);
            mockInputMapper.Setup(im => im.GetGamePadState(It.IsAny<GamePadState>())).Returns(mockGamePadCommands);

            var inputManager = new InputManager(mockInputMapper.Object);
            var handleCommandMock = new Mock<Action<BaseInputCommand>>();

            // Act
            inputManager.GetCommands(handleCommandMock.Object);

            // Assert
            handleCommandMock.Verify(hc => hc(It.IsAny<BaseInputCommand>()), Times.Once);
        }

        [Fact]
        public void GetCommands_ShouldInvokeHandleCommandForEachGamePadCommand()
        {
            // Arrange
            var mockInputMapper = new Mock<BaseInputMapper>();
            var mockKeyboardCommands = new List<BaseInputCommand>();
            var mockGamePadCommands = new List<BaseInputCommand> { new Mock<BaseInputCommand>().Object };

            mockInputMapper.Setup(im => im.GetKeyboardState(It.IsAny<KeyboardState>())).Returns(mockKeyboardCommands);
            mockInputMapper.Setup(im => im.GetGamePadState(It.IsAny<GamePadState>())).Returns(mockGamePadCommands);

            var inputManager = new InputManager(mockInputMapper.Object);
            var handleCommandMock = new Mock<Action<BaseInputCommand>>();

            // Act
            inputManager.GetCommands(handleCommandMock.Object);

            // Assert
            handleCommandMock.Verify(hc => hc(It.IsAny<BaseInputCommand>()), Times.Once);
        }

        [Fact]
        public void GetCommands_ShouldInvokeHandleCommandForBothKeyboardAndGamePadCommands()
        {
            // Arrange
            var mockInputMapper = new Mock<BaseInputMapper>();
            var mockKeyboardCommands = new List<BaseInputCommand> { new Mock<BaseInputCommand>().Object };
            var mockGamePadCommands = new List<BaseInputCommand> { new Mock<BaseInputCommand>().Object };

            mockInputMapper.Setup(im => im.GetKeyboardState(It.IsAny<KeyboardState>())).Returns(mockKeyboardCommands);
            mockInputMapper.Setup(im => im.GetGamePadState(It.IsAny<GamePadState>())).Returns(mockGamePadCommands);

            var inputManager = new InputManager(mockInputMapper.Object);
            var handleCommandMock = new Mock<Action<BaseInputCommand>>();

            // Act
            inputManager.GetCommands(handleCommandMock.Object);

            // Assert
            handleCommandMock.Verify(hc => hc(It.IsAny<BaseInputCommand>()), Times.Exactly(2));
        }
    }
}
