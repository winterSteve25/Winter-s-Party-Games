using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Object = UnityEngine.Object;

namespace Games.Base
{
    [Serializable]
    public class PlayerLobbySprite
    {
        private enum PrefabUseMode
        {
            SpawnInWorldSpace = 1,
            SpawnInScreenSpace = 3,
            UseSpriteAndAnimator = 2,
        }
        
        [DisableIf("@prefab != null"), SerializeField]
        private Sprite sprite;
        [DisableIf("@prefab != null"), SerializeField]
        private RuntimeAnimatorController controller;
        
        [SerializeField] private GameObject prefab;
        [SerializeField, ShowIf("@prefab != null")]
        private PrefabUseMode prefabUseMode;
        
        public GameObject Prefab => prefab;
        public bool WorldSpace => prefabUseMode == PrefabUseMode.SpawnInWorldSpace;
        
        [CanBeNull]
        public GameObject Build(Transform characterPrefabWorldSpaceParent, Transform characterPrefabParent, Image characterImage, Animator characterAnimator)
        {
            if (prefab == null)
            {
                characterImage.sprite = sprite;
                characterAnimator.runtimeAnimatorController = controller;
                return null;
            }

            if (prefab != null && prefabUseMode == PrefabUseMode.UseSpriteAndAnimator)
            {
                characterImage.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                characterAnimator.runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;
                return null;
            }

            var parent = prefabUseMode switch
            {
                PrefabUseMode.SpawnInWorldSpace => characterPrefabWorldSpaceParent,
                PrefabUseMode.SpawnInScreenSpace => characterPrefabParent,
                _ => throw new ArgumentOutOfRangeException()
            };
            var instantiated = Object.Instantiate(prefab, parent);
            
            if (characterPrefabParent != null)
            {
                instantiated.transform.position = characterPrefabParent.position;
                var followTransform = instantiated.AddComponent<FollowTransform>();
                followTransform.follow = characterPrefabParent;
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