using UnityEngine;

namespace Utils
{
    public class FollowTransform : MonoBehaviour
    {
        public Transform follow;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.position = follow.position;
            _transform.localScale = follow.localScale;
        }
    }
}