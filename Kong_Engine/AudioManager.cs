using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Kong_Engine
{
    public class AudioManager
    {
        private ContentManager _contentManager;
        private Dictionary<string, SoundEffect> _soundEffects;

        public AudioManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadSound(string name, string filePath)
        {
            var soundEffect = _contentManager.Load<SoundEffect>(filePath);
            _soundEffects[name] = soundEffect;
        }

        public void PlaySound(string name)
        {
            if (_soundEffects.TryGetValue(name, out var soundEffect))
            {
                soundEffect.Play();
            }
        }
    }
}
