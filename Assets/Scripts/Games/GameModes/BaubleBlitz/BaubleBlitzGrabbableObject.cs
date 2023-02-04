using System;
using Network.Sync;
using Photon.Pun;
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
        public bool Grabbed
        {
            get => _grabbed.Value;
            set => _grabbed.Value = value;
        }
        
        // The actor id was the player that is grabbing this object
        public int Grabber
        {
            get => _grabberID.Value;
            set => _grabberID.Value = value;
        }
        
        // The position that this object should follow when it is being grabbed.
        [NonSerialized] public Transform FollowPosition;
        
        // The rotation that this object should follow when it is being grabbed.
        [NonSerialized] public Transform FollowRotation;
        
        // An offset to apply to the position of this object when it is being grabbed.
        [NonSerialized] public Vector2 PositionOffset = Vector2.zero;
        
        // An offset to apply to the rotation of this object when it is being grabbed.
        [NonSerialized] public Quaternion RotationOffset = Quaternion.identity;
        
        // The default value for the SpeedReduction property. This can be changed in the Unity editor.
        protected float speedReduction = 0.5f;
        
        // Whether this object uses physics-based movement.
        private bool _physicsBased;
        
        // Internal network synced variables
        private SyncedVar<bool> _grabbed;
        private SyncedVar<int> _grabberID;

        // Initialize the RigidBody property and set the _physicsBased flag.
        protected virtual void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            _physicsBased = RigidBody != null;
            _grabbed = false;
            _grabberID = -1;
        }

        // Update the position and rotation of this object if it is being grabbed and not using physics-based movement.
        private void Update()
        {
            if (_physicsBased) return;
            if (!Grabbed) return;
            if (Grabber != PhotonNetwork.LocalPlayer.ActorNumber) return;
            transform.SetPositionAndRotation((Vector2) FollowPosition.position + PositionOffset, FollowRotation.rotation * RotationOffset);
        }

        // Update the position and rotation of this object if it is being grabbed and using physics-based movement.
        private void FixedUpdate()
        {
            if (!_physicsBased) return;
            if (!Grabbed) return;
            if (Grabber != PhotonNetwork.LocalPlayer.ActorNumber) return;
            RigidBody.MovePosition((Vector2) FollowPosition.position + PositionOffset);
            RigidBody.SetRotation(FollowRotation.rotation * RotationOffset);
        }
    }
}