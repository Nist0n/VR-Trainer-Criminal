using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryVR;
    
        [SerializeField] private GameObject anchor;

        [SerializeField] private InputActionProperty inventoryButton;

        [SerializeField] private Slot slot1;
        
        [SerializeField] private Slot slot2;
        
        [SerializeField] private Slot slot3;
        
        [SerializeField] private Slot slot4;
        
        [SerializeField] private GameObject brush;
        
        [SerializeField] private GameObject scotch;
    
        private bool _UIActive;

        private void Start()
        {
            slot1.InsertItem(brush);
            slot2.InsertItem(scotch);
            inventoryVR.SetActive(false);
            _UIActive = false;
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
    }
}
