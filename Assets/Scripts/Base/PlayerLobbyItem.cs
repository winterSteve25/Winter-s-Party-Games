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
        [SerializeField, Required] private Transform worldSpaceCharacterPrefabParent;

        private GameObject _spawnedPrefab;
        private TextMeshProUGUI _text;
        private RectTransform _textRectTransform;
        
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _textRectTransform = _text.GetComponent<RectTransform>();
            _textRectTransform.DOBlendableLocalMoveBy(new Vector3(0, 5, 0), 2f)
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
            var gameModePlayerAvatar = LobbyData.Instance.gameMode.playerAvatars[data.avatarIndex];
            _spawnedPrefab = gameModePlayerAvatar.Build(worldSpaceCharacterPrefabParent, characterPrefabParent, characterImage, characterAnimator);
        }

        public Vector3 GetCrownLocation()
        {
            return crownSpot.position;
        }

        public Quaternion GetCrownRotation()
        {
            return crownSpot.rotation;
        }

        public void ZoomIcon()
        {
            if (_spawnedPrefab != null)
            {
                _spawnedPrefab.transform.localScale = Vector3.zero;
                _spawnedPrefab.transform.DOScale(1, 0.1f)
                    .SetEase(Ease.OutCubic);
            }
            else
            {
                characterImage.transform.localScale = Vector3.zero;
                characterImage.transform.DOScale(1, 0.1f)
                    .SetEase(Ease.OutCubic);
            }
        }

        public void ShrinkIcon(Action restorePlayer)
        {
            if (_spawnedPrefab != null)
            {
                _spawnedPrefab.transform.DOScale(0, 0.1f)
                    .OnComplete(() => restorePlayer());
            }
            else
            {
                characterImage.transform.DOScale(0, 0.1f)
                    .OnComplete(() => restorePlayer());
            }
        }
    }
}