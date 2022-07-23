using UnityEngine;

namespace Menu
{
    public class ParticlesResizer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float z;
        
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            var width = 1 / (mainCamera.WorldToViewportPoint(new Vector3(1, 1, z)).x - 0.5f);
            var sh = _particleSystem.shape;
            sh.scale = new Vector3(width * 4, 1, 1);
        }
    }
}