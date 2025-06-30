using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace UI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private AdaptiveGridInventory adaptiveInventory;
        [SerializeField] private InventoryItemDatabase itemDatabase;
        
        [Header("Input Actions")]
        [SerializeField] private InputActionProperty toggleInventoryAction;
        [SerializeField] private InputActionProperty grabAction;
        
        private Dictionary<string, int> _playerInventory = new Dictionary<string, int>();
        private bool _isInitialized = false;
        
        public static InventoryManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeInventory();
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void InitializeInventory()
        {
            if (!itemDatabase)
            {
                itemDatabase = Resources.Load<InventoryItemDatabase>("InventoryItemDatabase");
            }
            
            if (adaptiveInventory && toggleInventoryAction.action != null)
            {
                adaptiveInventory.ToggleInventoryAction = toggleInventoryAction;
            }
            
            _isInitialized = true;
        }
        
        private void HandleInput()
        {
            if (!_isInitialized) return;
            
            if (toggleInventoryAction.action.triggered)
            {
                ToggleInventory();
            }
            
            if (grabAction.action.triggered)
            {
                HandleItemGrab();
            }
        }
        
        public void ToggleInventory()
        {
            Debug.Log("GGGGGGGGG");
            if (adaptiveInventory)
            {
                adaptiveInventory.ToggleInventory();
            }
        }
        
        private void HandleItemGrab()
        {
            // Логика захвата предметов из мира
        }
        
        public void SaveInventory()
        {
            string inventoryData = JsonUtility.ToJson(new InventoryData { items = _playerInventory });
            PlayerPrefs.SetString("PlayerInventory", inventoryData);
            PlayerPrefs.Save();
        }
        
        public void LoadInventory()
        {
            if (PlayerPrefs.HasKey("PlayerInventory"))
            {
                string inventoryData = PlayerPrefs.GetString("PlayerInventory");
                var data = JsonUtility.FromJson<InventoryData>(inventoryData);
                _playerInventory = data.items ?? new Dictionary<string, int>();
            }
        }
        
        [System.Serializable]
        private class InventoryData
        {
            public Dictionary<string, int> items;
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveInventory();
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveInventory();
            }
        }
    }
}