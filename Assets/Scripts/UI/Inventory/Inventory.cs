using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryVR;
    
        [SerializeField] private GameObject anchor;

        [SerializeField] private InputActionProperty inventoryButton;
    
        private bool _UIActive;

        private void Start()
        {
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
