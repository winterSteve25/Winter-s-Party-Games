using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Games.GameModes.BaubleBlitz
{
    // This class represents a shopping cart in the Bauble Blitz game mode.
    // It inherits from the BaubleBlitzGrabbableObject class, which allows it to be grabbed and moved around.
    public class BaubleBlitzShoppingCart : BaubleBlitzGrabbableObject
    {
        // The amount that the speed of this object is reduced when it is being grabbed.
        // The reduction is multiplied by the number of items on the cart + 1.
        public override float SpeedReduction => speedReduction * (ItemsOnCart.Count + 1);
        
        // The transform of the spot on the cart where items should be placed.
        public Transform ItemSpotOnCart => itemsSpotOnCart;
        
        // The maximum number of items that can be placed on the cart.
        public int ItemCountCap => itemCountCap;
        
        // A list of the items currently on the cart. This list is read-only and can only be modified through the AddItem and RemoveItem methods.
        [ShowInInspector, ReadOnly] public List<BaubleBlitzGrabbableObject> ItemsOnCart { get; private set; }
        
        // The transform of the spot on the cart where items should be placed. This is set in the Unity editor.
        [SerializeField] private Transform itemsSpotOnCart;
        
        // The maximum number of items that can be placed on the cart. This is set in the Unity editor.
        [SerializeField] private int itemCountCap = 4;

        // Initialize the ItemsOnCart property.
        protected override void Start()
        {
            base.Start();
            ItemsOnCart = new List<BaubleBlitzGrabbableObject>();
        }
    }
}