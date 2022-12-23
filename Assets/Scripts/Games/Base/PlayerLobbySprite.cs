using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Games.Base
{
    [Serializable]
    public class PlayerLobbySprite
    {
        [DisableIf("@prefab != null"), SerializeField]
        private Sprite sprite;
        [DisableIf("@prefab != null"), SerializeField]
        private RuntimeAnimatorController controller;
        
        [SerializeField] private GameObject prefab;
        [SerializeField, ShowIf("@prefab != null"), LabelWidth(300)]
        private bool spawnInWorldPlace;

        public GameObject Prefab => prefab;
        
        [CanBeNull]
        public GameObject Build(Transform characterPrefabWorldSpaceParent, Transform characterPrefabParent, Image characterImage, Animator characterAnimator)
        {
            if (prefab == null)
            {
                characterImage.sprite = sprite;
                characterAnimator.runtimeAnimatorController = controller;
                return null;
            }

            var instantiated = Object.Instantiate(prefab, spawnInWorldPlace ? characterPrefabWorldSpaceParent : characterPrefabParent);
            if (characterPrefabParent != null)
            {
                instantiated.transform.position = characterPrefabParent.position;
            }

            if (characterImage != null)
            {
                characterImage.gameObject.SetActive(false);
            }

            if (characterAnimator != null)
            {
                characterAnimator.enabled = false;
            }
            
            return instantiated;
        }
        
        
    }
}