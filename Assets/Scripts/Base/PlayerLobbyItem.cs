using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class PlayerLobbyItem : MonoBehaviour
    {
        public PlayerLobbyItemData data;

        [SerializeField, Required] private RectTransform crownSpot;
        [SerializeField, Required] private Image characterImage;
        [SerializeField, Required] private Animator characterAnimator;
        [SerializeField, Required] private Transform characterPrefabParent;

        private GameObject _spawnedPrefab;
        private TextMeshProUGUI _text;
        private RectTransform _textRectTransform;
        
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _textRectTransform = _text.GetComponent<RectTransform>();
            _textRectTransform.DOBlendableLocalMoveBy(new Vector3(0, 10, 0), 2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);

            if (LobbyData.Instance == null)
            {
                Debug.LogError("Player Lobby Item exist when LobbyData does not");
                Destroy(gameObject);
            }
        }
        
        public void UpdateAppearance()
        {
            if (data == null) return;
            GetComponentInChildren<TextMeshProUGUI>().text = data.nickname;
            var gameModePlayerAvatar = LobbyData.Instance.PartyGame.playerAvatars[data.avatarIndex];
            _spawnedPrefab = gameModePlayerAvatar.PlayerLobbySprite.Build(LobbyData.Instance.PlayerWorldSpaceDisplay, characterPrefabParent, characterImage, characterAnimator);
        }

        public Vector3 GetCrownLocation()
        {
            return crownSpot.position;
        }

        public Quaternion GetCrownRotation()
        {
            return crownSpot.rotation;
        }

        public void ZoomIcon(Vector3 beginScale)
        {
            // if (_spawnedPrefab != null)
            // {
            //     _spawnedPrefab.transform.localScale = beginScale;
            //     _spawnedPrefab.transform.DOScale(1, 0.1f)
            //         .SetEase(Ease.OutCubic);
            // }
            // else
            // {
                characterPrefabParent.localScale = beginScale;
                characterPrefabParent.DOScale(1, 0.1f)
                    .SetEase(Ease.OutCubic);
            // }
        }

        public void ZoomIcon()
        {
            // if (_spawnedPrefab != null)
            // {
                // _spawnedPrefab.transform.DOScale(1, 0.1f)
                    // .SetEase(Ease.OutCubic);
            // }
            // else
            // {
                characterPrefabParent.DOScale(1, 0.1f)
                    .SetEase(Ease.OutCubic);
            // }
        }
        
        public void ShrinkIcon(Action onComplete = null, float endScale = 0)
        {
            // if (_spawnedPrefab != null)
            // {
                // _spawnedPrefab.transform.DOScale(endScale, 0.1f)
                    // .OnComplete(() => onComplete?.Invoke());
            // }
            // else
            // {
                characterPrefabParent.DOScale(endScale, 0.1f)
                    .OnComplete(() => onComplete?.Invoke());
            // }
        }

        public void ChangeColor(Color color)
        {
            if (_spawnedPrefab != null)
            {
                var gameModePlayerAvatar = LobbyData.Instance.PartyGame.playerAvatars[data.avatarIndex];
                if (gameModePlayerAvatar.PlayerLobbySprite.WorldSpace)
                {
                    foreach (var spriteRenderer in _spawnedPrefab.GetComponentsInChildren<SpriteRenderer>())
                    {
                        spriteRenderer.color = color;
                    }
                }
                else
                {
                    foreach (var spriteRenderer in _spawnedPrefab.GetComponentsInChildren<Image>())
                    {
                        spriteRenderer.color = color;
                    }
                }
            }
            else
            {
                characterImage.color = color;
            }
        }

        public void Destroy()
        {
            if (_spawnedPrefab != null) Destroy(_spawnedPrefab);
            Destroy(gameObject);
        }
    }
}