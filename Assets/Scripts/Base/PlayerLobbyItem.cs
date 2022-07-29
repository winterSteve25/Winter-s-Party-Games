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
            var gameModePlayerAvatar = LobbyData.Instance.gameMode.playerAvatars[data.avatarIndex];
            var prefab = gameModePlayerAvatar.prefab;

            Sprite sprite;
            RuntimeAnimatorController controller;

            if (prefab is not null)
            {
                sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                controller = prefab.GetComponentInChildren<Animator>().runtimeAnimatorController;
            }
            else
            {
                sprite = gameModePlayerAvatar.sprite;
                controller = gameModePlayerAvatar.controller;
            }
            
            GetComponentInChildren<Image>().sprite = sprite;

            if (controller is null)
            {
                GetComponent<Animator>().enabled = false;    
                return;
            }
            
            GetComponent<Animator>().runtimeAnimatorController = controller;
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