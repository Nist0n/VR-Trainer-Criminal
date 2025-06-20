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
    
        private bool _uiActive;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != "Fabula1") return;
            
            slots[0].InsertItem(brush);
            slots[1].InsertItem(scotch);
            inventoryVR.SetActive(false);
            _uiActive = false;
        }

        private void Update()
        {
            if (!inventoryButton.action.triggered) return;
            
            _uiActive = !_uiActive;
            inventoryVR.SetActive(_uiActive);
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
