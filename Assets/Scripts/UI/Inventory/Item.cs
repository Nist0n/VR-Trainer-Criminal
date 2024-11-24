using UnityEngine;

namespace UI.Inventory
{
    public class Item : MonoBehaviour
    {
        public bool InSlot;

        public Vector3 SlotRotation = Vector3.zero;

        public Slot CurrentSlot;

        public bool IsHanded;

        public void CheckUI()
        {
            if (InSlot)
            {
                Debug.Log("Entered");
                IsHanded = false;
                CurrentSlot.ItemInSlot = null;
                gameObject.transform.parent = null;
                InSlot = false;
                CurrentSlot.ResetColor();
                CurrentSlot = null;
            }
        }

        public void IsHandedFalse()
        {
            IsHanded = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
