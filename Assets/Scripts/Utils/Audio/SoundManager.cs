using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Utils.Audio
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;

        [SerializeField] private SoundMap soundMap;
        [SerializeField] private SoundCategoryMap categoryMap;

        private Dictionary<GameConstants.SoundCategory, AudioSource> _internalSourceMapper;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }

        private void Start()
        {
            _internalSourceMapper = new Dictionary<GameConstants.SoundCategory, AudioSource>();
            
            foreach (var (k, v) in categoryMap)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.outputAudioMixerGroup = v;
                _internalSourceMapper.Add(k, source);
            }
        }

        private void PlayInternal(GameConstants.Sounds sound)
        {
            var s = soundMap[sound];
            var source = _internalSourceMapper[s.category];

            if (source.isPlaying)
            {
                if (s.interrupt)
                {
                    source.DOFade(0, 0.2f).OnComplete(() =>
                    {
                        source.Stop();
                        Play(s, source);
                    });
                }
            }
            else
            {
                Play(s, source);
            }
        }

        private static void Play(Sound s, AudioSource source)
        {
            source.clip = s.clip;
            source.loop = s.loop;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.Play();
        }
        
        public static void Play(GameConstants.Sounds sound)
        {
            _instance.PlayInternal(sound);
        }
    }

    [Serializable]
    public class SoundMap : SerializableDictionary<GameConstants.Sounds, Sound>
    {
    }
    
    [Serializable]
    public class SoundCategoryMap : SerializableDictionary<GameConstants.SoundCategory, AudioMixerGroup>
    {
    }
}