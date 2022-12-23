using KevinCastejon.MoreAttributes;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menu
{
    [RequireComponent(typeof(Button))]
    public class BackButton : MonoBehaviour
    {
        [Scene, SerializeField] private string toScene;
        
        [SerializeField] private bool leaveRoom;

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
            if (PhotonNetwork.InRoom)
            {
                if (leaveRoom)
                {
                    PhotonNetwork.LeaveRoom();
                } 
            }

            SceneManager.TransitionToScene(toScene);
        }
    }
}