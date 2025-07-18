using System;
using UnityEngine;

namespace UI.Inventory
{
    public class PickupableFingerprint : PickupableItem
    {
        public string SurfaceName;
        public DateTime? TimeOfPhoto;
        public void ItemID(string id)
        {
            itemId = id;
            Debug.Log(itemId + " Поставленный ID");
        }
    }
}
