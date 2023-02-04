using System;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Games.GameModes.BaubleBlitz
{
    // This script is attached to the player character in the BaubleBlitz game mode
    public class BaubleBlitzPlayerController : MonoBehaviour
    {
        // The speed at which the player character moves
        [SerializeField] private float speed = 4f;
        // The speed at which the player character rotates
        [SerializeField] private float rotateSpeed = 4f;
        // The distance at which the player character can pick up and drop off objects
        [SerializeField] private float handleRange = 2f;
        // The transform of the spot where grabbed objects should be placed when carried by the player character
        [SerializeField] private Transform grabItemSpot;
        
        // Reference to the Rigidbody2D component on the player character game object
        private Rigidbody2D _rigidbody2D;
        // Reference to the Collider2D component on the player character game object
        private Collider2D _collider2D;
        // Vector2 used to store the player character's movement direction
        private Vector2 _dir;
        // Photon View 
        private PhotonView _photonView;
        
        // Reference to the object currently being grabbed by the player character
        private BaubleBlitzGrabbableObject _grabbingObject;
        // Reference to the parent transform of the object that was grabbed before it was picked up by the player character
        private Transform _originalObjectParent;
        
        // The actual speed of the player character, taking into account any speed reduction from grabbed objects
        [SerializeField, ReadOnly]
        private float actualSpeed;

        private void Start()
        {
            // Get references to the Rigidbody2D and Collider2D components on the player character game object
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            // Initialize the _dir vector to (0, 0)
            _dir = new Vector2();
            _photonView = GetComponent<PhotonView>();
            // Set the actual speed of the player character to the base speed
            actualSpeed = speed;
        }

        private void Update()
        {
            if (!_photonView.IsMine) return;
            
            // Set the _dir vector based on the horizontal and vertical input axes
            _dir.x = Input.GetAxis("Horizontal");
            _dir.y = Input.GetAxis("Vertical");
            // Normalize the _dir vector to ensure it has a length of 1
            _dir.Normalize();

            // If the E key is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // If the player character is not currently grabbing an object
                if (_grabbingObject == null)
                {
                    // Get the closest BaubleBlitzGrabbableObject that is not already grabbed
                    // ReSharper disable once Unity.PreferNonAllocApi
                    var closestGrabbable = GetClosestObjectOfType<BaubleBlitzGrabbableObject>(obj => !obj.Grabbed, null);

                    // If a closestGrabbable object was found
                    if (closestGrabbable != null)
                    {
                        // Set the _grabbingObject reference to the closestGrabbable object
                        _grabbingObject = closestGrabbable;
                        
                        // Cache transform
                        var thisTransform = transform;
                        
                        // Make the currently grabbed object to follow the player's position and rotation
                        _grabbingObject.FollowPosition = grabItemSpot;
                        _grabbingObject.FollowRotation = thisTransform;
                        _grabbingObject.Grabbed = true;
                        _grabbingObject.Grabber = PhotonNetwork.LocalPlayer.ActorNumber;
                        _originalObjectParent = _grabbingObject.transform.parent;

                        // Update the hierarchy
                        _grabbingObject.transform.SetParent(thisTransform);
                        _grabbingObject.transform.SetPositionAndRotation(grabItemSpot.position, thisTransform.rotation);

                        // Update the actual speed to reflect speed reduction
                        actualSpeed = speed - _grabbingObject.SpeedReduction;
                    }
                }
                else
                {
                    // Check if currently grabbing a shopping cart
                    if (_grabbingObject is not BaubleBlitzShoppingCart)
                    {
                        // If not we can try to find one nearby  
                        var shoppingCartNearby = GetClosestObjectOfType<BaubleBlitzShoppingCart>(obj => obj.ItemsOnCart.Count < obj.ItemCountCap, null);
                        if (shoppingCartNearby != null)
                        {
                            // If it is found we add the current carrying object to the shopping cart 
                            var grabbedObjectTransform = _grabbingObject.transform;
                            var cartTransform = shoppingCartNearby.transform;

                            _grabbingObject.FollowPosition = shoppingCartNearby.ItemSpotOnCart;
                            _grabbingObject.FollowRotation = cartTransform;
                            _grabbingObject.Grabbed = true;
                            _grabbingObject.Grabber = PhotonNetwork.LocalPlayer.ActorNumber;

                            grabbedObjectTransform.SetParent(cartTransform);
                            grabbedObjectTransform.position = shoppingCartNearby.transform.position;
                            shoppingCartNearby.ItemsOnCart.Add(_grabbingObject);

                            _grabbingObject = null;
                            actualSpeed = speed;
                            return;
                        }
                    }
                    
                    // If currently grabbing a shopping cart, we release it.
                    _grabbingObject.transform.SetParent(_originalObjectParent);
                    _grabbingObject.Grabbed = false;
                    _grabbingObject.Grabber = -1;
                    _grabbingObject.FollowPosition = null;
                    _grabbingObject.FollowRotation = null;
                    _grabbingObject = null;
                    actualSpeed = speed;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_photonView.IsMine) return;
            
            // If the movement direction is zero we return
            if (_dir == Vector2.zero) return;
            
            // Update player rotation and position
            var transform1 = transform;
            _rigidbody2D.MovePosition(_rigidbody2D.position + _dir * (actualSpeed * Time.fixedDeltaTime));
            _rigidbody2D.MoveRotation(Quaternion.RotateTowards(transform1.rotation, Quaternion.LookRotation(transform1.forward, _dir), rotateSpeed * Time.fixedDeltaTime));
        }

        /// <summary>
        /// This function returns the closest object of a specific type (T) that satisfies a given condition.
        /// If no such object is found, the default value is returned.
        /// </summary>
        /// <param name="condition">The condition that the object must satisfy in order to be considered a valid candidate.</param>
        /// <param name="defaultValue">The value to return if no object is found that satisfies the given condition.</param>
        /// <returns>The closest object of type T that satisfies the given condition, or the default value if none was found.</returns>
        private T GetClosestObjectOfType<T>(Predicate<T> condition, T defaultValue)
        {
            // Find all colliders within the handle range of the current object
            var colliders = Physics2D.OverlapCircleAll(transform.position, handleRange);

            // Initialize variables to track the closest collider and the closest object of type T
            Collider2D closest = null;
            var closestOfType = defaultValue;

            // Iterate through all colliders
            foreach (var col in colliders)
            {
                // Try to get the object of type T from the collider
                if (!col.TryGetComponent<T>(out var obj)) continue;

                // If the object doesn't satisfy the given condition, skip it
                if (!condition(obj)) continue;
                        
                // If this is the first object of type T that satisfies the condition, or if it's closer than the previous closest object,
                // update the closest object variables
                if (closest == null || col.Distance(_collider2D).distance < closest.Distance(_collider2D).distance)
                {
                    closest = col;
                    closestOfType = obj;
                }
            }

            // Return the closest object of type T that satisfies the condition, or the default value if none was found
            return closestOfType;
        }
    }
}