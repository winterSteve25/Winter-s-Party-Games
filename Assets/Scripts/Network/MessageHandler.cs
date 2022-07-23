using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public class MessageHandler : MonoBehaviour, IOnEventCallback
    {
        private static MessageHandler _instance;
        
        private Dictionary<byte, IMessage> _messages;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _messages = new Dictionary<byte, IMessage>();
                DontDestroyOnLoad(gameObject);
                return;
            }
        
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            var photonEventCode = photonEvent.Code;
            if (photonEventCode >= _messages.Count) return;
            _messages[photonEventCode].Handle(photonEvent);
        }

        private static byte _messageID;

        public static readonly RaiseEventOptions Others = new()
        {
            Receivers = ReceiverGroup.Others
        };

        public static readonly RaiseEventOptions All = new()
        {
            Receivers = ReceiverGroup.All
        };
        
        public static byte RegisterMessage(IMessage message)
        {
            _instance._messages.Add(_messageID, message);
            return _messageID++;
        }

        public static void UnregisterMessage(byte id)
        {
            _instance._messages.Remove(id);
        }

        public static void SendMessage(IMessage messages)
        {
            SendMessage(messages, Others, SendOptions.SendReliable);
        }
        
        public static void SendMessage(IMessage message, RaiseEventOptions options, SendOptions sendOptions)
        {
            var msg = _instance._messages.First(kv => kv.Value.GetType() == message.GetType());
            PhotonNetwork.RaiseEvent(msg.Key, message.Send(), options, sendOptions);
        }
    }
}