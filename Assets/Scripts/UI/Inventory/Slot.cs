using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class Slot : MonoBehaviour
    {
        public GameObject ItemInSlot;

        public Image SlotImage;

        private Color _originalColor;
    
        [SerializeField] private InputActionProperty putItem;
        
        private void Start()
        {
            SlotImage = GetComponentInChildren<Image>();

            _originalColor = SlotImage.color;
        }
        
        private void OnTriggerStay(Collider other)
        {
            Debug.Log("XD");
            
            if (ItemInSlot != null) return;

            GameObject obj = other.gameObject;

            if (!IsItem(obj))
            {
                return;
            }
            
            if (!putItem.action.IsPressed() && obj.GetComponent<Item>().IsHanded)
            {
                InsertItem(obj);
            }
        }

        private bool IsItem(GameObject obj)
        {
            if (obj.GetComponent<Item>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InsertItem(GameObject obj)
        {
            Debug.Log("Insert");
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = obj.GetComponent<Item>().SlotRotation;
            obj.GetComponent<Item>().InSlot = true;
            obj.GetComponent<Item>().CurrentSlot = this;
            ItemInSlot = obj;
            SlotImage.color = Color.gray;
        }

        public void ResetColor()
        {
            SlotImage.color = _originalColor;
        }
    }
}
