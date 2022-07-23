using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Utils;

namespace Network.Scenes
{
    public class CreateConnection : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI text;
        private Coroutine _coroutine;

        private void Start()
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString(GameConstants.PlayerPrefs.Username);
            PhotonNetwork.ConnectUsingSettings();
            _coroutine = StartCoroutine(ChangeText());
        }

        private IEnumerator ChangeText()
        {
            while (true)
            {
                text.text += ".";
                yield return new WaitForSeconds(1f);
                text.text += ".";
                yield return new WaitForSeconds(1f);
                text.text += ".";
                yield return new WaitForSeconds(1f);
                text.text = text.text[..^3];
                yield return new WaitForSeconds(2f);
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            StopCoroutine(_coroutine);
            SceneTransition.TransitionToScene(GameConstants.SceneIndices.MainMenu);
        }
    }
}