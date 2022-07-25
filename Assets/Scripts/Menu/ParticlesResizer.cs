using System;
using UnityEngine;
using Utils;

namespace Menu
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesResizer : MonoBehaviour
    {
        private static ParticlesResizer _instance;
        
        private ParticleSystem _particleSystem;
        
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float z;

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
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            SceneTransition.OnTransitionedToNewScene += NewScene;
        }

        private void OnDisable()
        {
            SceneTransition.OnTransitionedToNewScene -= NewScene;
        }

        private void Update()
        {
            var width = 1 / (mainCamera.WorldToViewportPoint(new Vector3(1, 1, z)).x - 0.5f);
            var sh = _particleSystem.shape;
            sh.scale = new Vector3(width * 4, 1, 1);
        }

        private void NewScene()
        {
            mainCamera = Camera.main;
        }
    }
}