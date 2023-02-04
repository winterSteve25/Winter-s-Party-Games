using System;
using UnityEngine;

namespace Utils
{
    public class FollowMouse : MonoBehaviour
    {
        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool changePivot; 
        
        private RectTransform _rectTransform;
        private Transform _transform;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _transform = transform;
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var mousePosition = Input.mousePosition;

            switch (parentCanvas.renderMode)
            {
                case RenderMode.ScreenSpaceCamera:
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentCanvas.transform as RectTransform, mousePosition,
                        parentCanvas.worldCamera, out var pos);
            
                    _transform.position = parentCanvas.transform.TransformPoint(pos + offset);
                    break;
                case RenderMode.ScreenSpaceOverlay:
                {
                    _transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

                    if (!changePivot) break;
                    
                    var pivotX = mousePosition.x / Screen.width;
                    var pivotY = mousePosition.y / Screen.height;
                
                    _rectTransform.pivot = new Vector2(Mathf.RoundToInt(pivotX), Mathf.RoundToInt(pivotY));
                    break;
                }
                case RenderMode.WorldSpace:
                    Debug.LogWarning("World space canvas not supported!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}