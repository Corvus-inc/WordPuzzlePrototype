using UnityEngine;

namespace Game
{
    public class MusicManager : IMusicManager
    {
        private readonly AudioSource _audioSource;

        public MusicManager(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void PlayMusic()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        public bool IsMusicEnabled => _audioSource.isPlaying;
    }
}