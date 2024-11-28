using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryVR;
    
        [SerializeField] private GameObject anchor;

        [SerializeField] private InputActionProperty inventoryButton;

        [SerializeField] private List<Slot> slots;
        
        [SerializeField] private GameObject brush;
        
        [SerializeField] private GameObject scotch;
    
        private bool _UIActive;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "Fabula1")
            {
                slots[0].InsertItem(brush);
                slots[1].InsertItem(scotch);
                inventoryVR.SetActive(false);
                _UIActive = false;
            }
        }

        private void Update()
        {
            if (inventoryButton.action.triggered)
            {
                _UIActive = !_UIActive;
                inventoryVR.SetActive(_UIActive);
            }
            // if (_UIActive)
            // {
            //     inventoryVR.transform.position = anchor.transform.position;
            //     inventoryVR.transform.eulerAngles = new Vector3(anchor.transform.eulerAngles.x + 15, anchor.transform.eulerAngles.y, 0);
            // }
        }

        public Slot CheckEmpty()
        {
            foreach (var slot in slots)
            {
                if (slot.ItemInSlot == null)
                {
                    return slot;
                }
            }

            return null;
        }
    }
}
