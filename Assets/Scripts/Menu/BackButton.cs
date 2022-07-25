using System;
using Photon.Pun;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menu
{
    [RequireComponent(typeof(Button))]
    public class BackButton : MonoBehaviour
    {
        [SerializeField] private GameConstants.SceneIndices toScene;
        
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

            SceneTransition.TransitionToScene(toScene);
        }
    }
}