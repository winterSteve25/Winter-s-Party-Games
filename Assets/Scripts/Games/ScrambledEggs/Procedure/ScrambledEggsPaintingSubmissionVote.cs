using Games.Base;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Games.ScrambledEggs.Procedure
{
    [RequireComponent(typeof(Button))]
    public class ScrambledEggsPaintingSubmissionVote : MonoBehaviour
    {
        [SerializeField] private RectTransform voteOption;
        private int _index;

        public void Init(int index, Texture2D painting, string sentence)
        {
            _index = index;
            GetComponent<Image>().sprite = Sprite.Create(painting, new Rect(0, 0, painting.width, painting.height), new Vector2(0.5f, 0.5f));
            GetComponentInChildren<TextMeshProUGUI>().text = sentence.Replace("\r", "");
        }
        
        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            FindObjectOfType<VoteManager>().Vote(PhotonNetwork.LocalPlayer.ActorNumber, _index, voteOption.position, voteOption.rotation);
            GetComponent<Button>().interactable = false;
        }
    }
}