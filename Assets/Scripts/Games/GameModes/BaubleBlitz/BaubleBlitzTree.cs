using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.GameModes.BaubleBlitz
{
    public class BaubleBlitzTree : MonoBehaviour
    {
        private static readonly int ShakeLeft = Animator.StringToHash("Shake Left");
        private static readonly int ShakeRight = Animator.StringToHash("Shake Right");
        
        [SerializeField] private List<Transform> ornamentSpots;
        [SerializeField] private Animator animator;

        private List<GameObject> _ornaments;

        private void Start()
        {
            _ornaments = new List<GameObject>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.TryGetComponent<BaubleBlitzShoppingCart>(out var shoppingCart))
            {
                if ((col.collider.transform.position - col.otherCollider.transform.position).x > 0)
                {
                    ShakeLeftward();
                }
                else
                {
                    ShakeRightward();
                }
                
                foreach (var ornament in shoppingCart.ItemsOnCart)
                {
                    var randomSpot = ornamentSpots[Random.Range(0, ornamentSpots.Count)];
                    ornamentSpots.Remove(randomSpot);
                    var ornamentGo = ornament.gameObject;
                    ornamentGo.transform.position = randomSpot.position;
                    ornament.enabled = false;
                    _ornaments.Add(ornamentGo);
                    ornamentGo.transform.SetParent(randomSpot.parent);
                    ornamentGo.transform.rotation = Quaternion.identity;
                }

                shoppingCart.ItemsOnCart.Clear();
            }
        }
        
        private void ShakeLeftward()
        {
            animator.SetTrigger(ShakeLeft);
        }

        private void ShakeRightward()
        {
            animator.SetTrigger(ShakeRight);
        }
    }
}