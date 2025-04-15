using UnityEngine;

namespace Game
{
    public interface IMusicManager
    {
         
        void PlayMusic();
        void StopMusic();
        bool IsMusicEnabled { get; }
    }
}