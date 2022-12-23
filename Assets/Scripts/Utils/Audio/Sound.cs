using System;
using UnityEngine;
using Utils.Data;

namespace Utils.Audio
{
    [Serializable]
    public class Sound
    {
        public AudioClip clip;
        
        [Range(0, 1)]
        public float volume = 1;
        
        [Range(-3, 3)]
        public float pitch = 1;
        
        public GameConstants.SoundCategory category;

        public bool interrupt;

        public bool loop;
    }
}