using System;
using UnityEngine;

namespace UI.Inventory
{
    public class Item : MonoBehaviour
    {
        public bool InSlot;

        public Vector3 SlotRotation = Vector3.zero;

        public Slot CurrentSlot;

        public bool IsHanded;

        private Vector3 _originalScale;

        public void CheckUI(GameObject joy)
        {
            joy.GetComponent<Item>().IsHanded = false;
            
            if (!InSlot) return;
            
            joy.GetComponent<Item>().CurrentSlot.ItemInSlot = null;
            joy.transform.parent = null;
            joy.GetComponent<Item>().InSlot = false;
            joy.GetComponent<Item>().CurrentSlot.ResetColor();
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            joy.GetComponent<Item>().CurrentSlot = null;
        }

        public void IsHandedFalse()
        {
            IsHanded = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.transform.parent = null;
        }
    }
}
