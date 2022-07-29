using System;
using UnityEngine;

namespace Games.Base
{
    [Serializable]
    public class PlayerLobbySprite
    {
        public Sprite sprite;
        public RuntimeAnimatorController controller;
        public GameObject prefab;
    }
}