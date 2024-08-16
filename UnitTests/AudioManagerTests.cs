using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Kong_Engine.Tests
{
    public class AudioManagerTests
    {
        [Fact]
        public void LoadSound_ShouldAddSoundEffectToDictionary()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockContentManager = new Mock<ContentManager>(MockBehavior.Strict, mockServiceProvider.Object);
            var mockSoundEffect = new Mock<SoundEffect>();
            mockContentManager.Setup(cm => cm.Load<SoundEffect>("Content/splashSound")).Returns(mockSoundEffect.Object);

            var audioManager = new AudioManager(mockContentManager.Object);

            // Act
            audioManager.LoadSound("splashSound", "Content/splashSound");

            // Assert
            mockContentManager.Verify(cm => cm.Load<SoundEffect>("Content/splashSound"), Times.Once);
        }

        [Fact]
        public void PlaySound_ShouldPlaySoundEffect()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockContentManager = new Mock<ContentManager>(MockBehavior.Strict, mockServiceProvider.Object);
            var mockSoundEffect = new Mock<SoundEffect>();
            mockSoundEffect.Setup(se => se.Play()).Verifiable();

            var audioManager = new AudioManager(mockContentManager.Object);
            audioManager.LoadSound("splashSound", "Content/splashSound");

            // Injecting mock data directly into the sound effects dictionary
            var soundEffectsField = typeof(AudioManager).GetField("_soundEffects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Ensure soundEffectsField is not null
            Assert.NotNull(soundEffectsField);

            // Safely get the value of the field, handling potential null values
            var soundEffects = soundEffectsField?.GetValue(audioManager) as Dictionary<string, SoundEffect>;

            // Ensure soundEffects is not null
            Assert.NotNull(soundEffects);

            if (soundEffects != null)
            {
                soundEffects["splashSound"] = mockSoundEffect.Object;
            }

            // Act
            audioManager.PlaySound("splashSound");

            // Assert
            mockSoundEffect.Verify(se => se.Play(), Times.Once);
        }
    }
}
