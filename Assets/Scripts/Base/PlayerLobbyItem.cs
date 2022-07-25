using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class PlayerLobbyItem : MonoBehaviour
    {
        public PlayerLobbyItemData data;

        [SerializeField] private RectTransform crownSpot;
        [SerializeField] private AnimationCurve bobMotion;
        private TextMeshProUGUI _text;
        private RectTransform _textRectTransform;
        
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _textRectTransform = _text.GetComponent<RectTransform>();
            _textRectTransform.DOBlendableLocalMoveBy(new Vector3(0, 5, 0), 2f).SetLoops(-1, LoopType.Yoyo).SetEase(bobMotion);

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
            GetComponentInChildren<Image>().sprite = LobbyData.Instance.gameMode.playerAvatars[data.avatarIndex];
        }

        public Vector3 GetCrownLocation()
        {
            return crownSpot.position;
        }

        public Quaternion GetCrownRotation()
        {
            return crownSpot.rotation;
        } 
    }
}