using System;
using UnityEngine;

namespace Games.GameModes.BaubleBlitz
{
    // This class represents a "grabbable object" in the Bauble Blitz game mode.
    public class BaubleBlitzGrabbableObject : MonoBehaviour
    {
        // The rigidbody component of this object. This is used to apply physics-based movement.
        public Rigidbody2D RigidBody { get; private set; }

        // The amount that the speed of this object is reduced when it is being grabbed.
        public virtual float SpeedReduction => speedReduction;

        // Whether this object is currently being grabbed.
        [NonSerialized] public bool Grabbed;
        
        // The position that this object should follow when it is being grabbed.
        [NonSerialized] public Transform FollowPosition;
        
        // The rotation that this object should follow when it is being grabbed.
        [NonSerialized] public Transform FollowRotation;
        
        // An offset to apply to the position of this object when it is being grabbed.
        [NonSerialized] public Vector2 PositionOffset = Vector2.zero;
        
        // An offset to apply to the rotation of this object when it is being grabbed.
        [NonSerialized] public Quaternion RotationOffset = Quaternion.identity;
        
        // The default value for the SpeedReduction property. This can be changed in the Unity editor.
        [SerializeField] protected float speedReduction = 0.5f;
        
        // Whether this object uses physics-based movement.
        private bool _physicsBased;

        // Initialize the RigidBody property and set the _physicsBased flag.
        protected virtual void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            _physicsBased = RigidBody != null;
        }

        // Update the position and rotation of this object if it is being grabbed and not using physics-based movement.
        private void Update()
        {
            if (_physicsBased) return;
            if (!Grabbed) return;
            transform.SetPositionAndRotation((Vector2) FollowPosition.position + PositionOffset, FollowRotation.rotation * RotationOffset);
        }

        // Update the position and rotation of this object if it is being grabbed and using physics-based movement.
        private void FixedUpdate()
        {
            if (!_physicsBased) return;
            if (!Grabbed) return;
            RigidBody.MovePosition((Vector2) FollowPosition.position + PositionOffset);
            RigidBody.SetRotation(FollowRotation.rotation * RotationOffset);
        }
    }
}