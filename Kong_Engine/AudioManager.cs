using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Kong_Engine
{
    public class AudioManager
    {
        private ContentManager _contentManager;
        private Dictionary<string, SoundEffect> _soundEffects;
        private Dictionary<string, Song> _songs;

        public AudioManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _soundEffects = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
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

        public void LoadSong(string name, string filePath)
        {
            var song = _contentManager.Load<Song>(filePath);
            _songs[name] = song;
        }

        public void PlaySong(string name, bool isRepeating = false)
        {
            if (_songs.TryGetValue(name, out var song))
            {
                MediaPlayer.IsRepeating = isRepeating;
                MediaPlayer.Play(song);
            }
        }
    }
}
