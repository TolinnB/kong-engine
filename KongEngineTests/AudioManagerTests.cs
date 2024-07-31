using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Moq;

namespace KongEngineTests
{
    [TestClass]
    public class AudioManagerTests
    {
        [TestMethod]
        public void PlaySound_ShouldPlayLoadedSound()
        {
            var contentManagerMock = new Mock<ContentManager>();
            var soundEffectMock = new Mock<SoundEffect>();
            contentManagerMock.Setup(cm => cm.Load<SoundEffect>("testSound")).Returns(soundEffectMock.Object);

            var audioManager = new AudioManager(contentManagerMock.Object);
            audioManager.LoadSound("testSound", "testSound");

            audioManager.PlaySound("testSound");

            soundEffectMock.Verify(se => se.Play(), Times.Once);
        }
    }
}
