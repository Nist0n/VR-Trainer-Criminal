using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI.Inventory
{
    public class AdaptiveGridInventory : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private List<GameObject> gridContainers;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button instrumentButton;
        [SerializeField] private Button evidenceButton;
        [SerializeField] private Button notebookButton;
        [SerializeField] private GameObject adaptiveGridInventory;
        
        
        [Header("Input")]
        [SerializeField]
        public InputActionProperty ToggleInventoryAction;
        
        [Header("Spawn Settings")]
        [SerializeField] private Transform spawnPoint;
        
        [Header("Item Return")]
        [SerializeField] private LayerMask itemLayerMask = -1;
        [SerializeField] private float pickupDistance = 2f;
        [SerializeField] private InputActionProperty pickupAction;
        
        [SerializeField] private List<GameObject> categoryTabs;
        private ToolCategory _currentCategory;
        private bool _isInventoryOpen;
        private List<InventorySlot> _slots = new List<InventorySlot>();
        
        private void Start()
        {
            returnButton.onClick.AddListener(ReturnToMainMenu);
            instrumentButton.onClick.AddListener(() => OnCategoryTabClicked(ToolCategory.Sampling));
            evidenceButton.onClick.AddListener(() => OnCategoryTabClicked(ToolCategory.Analysis));
            notebookButton.onClick.AddListener(() => OnCategoryTabClicked(ToolCategory.Documentation));
            LoadInitialItems();
            adaptiveGridInventory.SetActive(false);
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            if (pickupAction.action.triggered || Input.GetKeyDown(KeyCode.Q))
            {
                TryPickupItem();
            }
        }
        
        private void OnCategoryTabClicked(ToolCategory category)
        {
            EnterCategory(category);
        }
        
        private void EnterCategory(ToolCategory category)
        {
            foreach (var tab in categoryTabs)
            {
                tab.SetActive(false);
            }

            foreach (var container in gridContainers)
            {
                if (container.name.Contains(category.ToString()))
                {
                    container.SetActive(true);
                }
            }
            
            ShowReturnButton();
            
            Debug.Log($"Вошли в категорию: {category}");
        }
        
        private void ReturnToMainMenu()
        {
            ShowAllCategories();
            
            HideReturnButton();
            
            HideGridContainers();
        }
        
        private void ShowAllCategories()
        {
            foreach (var tab in categoryTabs)
            {
                tab.SetActive(true);
            }
        }
        
        private void ShowReturnButton()
        {
            returnButton.gameObject.SetActive(true);
        }
        
        private void HideReturnButton()
        {
            returnButton.gameObject.SetActive(false);
        }
        
        private void HideGridContainers()
        {
            foreach (var container in gridContainers)
            {
                container.SetActive(false);
            }
        }
        
        private void LoadInitialItems()
        {
            ShowAllCategories();
            
            var itemDatabase = Resources.Load<InventoryItemDatabase>("InventoryItemDatabase");
            if (!itemDatabase)
            {
                Debug.LogError("Failed to load InventoryItemDatabase from Resources!");
                return;
            }
            
            foreach (ToolCategory category in System.Enum.GetValues(typeof(ToolCategory)))
            {
                var items = itemDatabase.GetItemsByCategory(category);
                foreach (var item in items)
                {
                    AddItem(item);
                }
            }
        }
        
        public void AddItem(InventoryItem item)
        {
            if (item.isStackable)
            {
                InventorySlot existingSlot = FindSlotWithItem(item.itemId);
                if (existingSlot)
                {
                    existingSlot.AddToStack();
                    return;
                }
            }
            
            AddItemToSlot(item);
        }
        
        private void AddItemToSlot(InventoryItem item)
        {
            foreach (var container in gridContainers)
            {
                if (container.name.Contains(item.category.ToString()))
                {
                    Debug.Log("Так быть должно");
                    InventorySlot slot = Instantiate(slotPrefab, container.transform).GetComponent<InventorySlot>();
                    slot.SetItem(item, spawnPoint);
                    _slots.Add(slot);
                    Resources.Load<InventoryItemDatabase>("InventoryItemDatabase").AddItem(item);
                    break;
                }
            }
        }
        
        private InventorySlot FindSlotWithItem(string itemId)
        {
            foreach (var slot in _slots)
            {
                if (slot.Item && slot.Item.itemId == itemId)
                    return slot;
            }
            
            return null;
        }
        
        public void ToggleInventory()
        {
            _isInventoryOpen = !_isInventoryOpen;
            
            adaptiveGridInventory.SetActive(_isInventoryOpen);
            
            if (_isInventoryOpen)
            {
                ReturnToMainMenu();
            }
        }
        
        private void TryPickupItem()
        {
            Vector3 playerPosition = Camera.main.transform.position;
            Vector3 playerForward = Camera.main.transform.forward;
            
            RaycastHit hit;
            if (Physics.Raycast(playerPosition, playerForward, out hit, pickupDistance, itemLayerMask))
            {
                GameObject itemObject = hit.collider.gameObject;
                
                var pickupableItem = itemObject.GetComponent<PickupableItem>();
                if (pickupableItem)
                {
                    PickupItem(pickupableItem);
                }
            }
        }
        
        private void PickupItem(PickupableItem pickupableItem)
        {
            var inventoryItemDatabase = Resources.Load<InventoryItemDatabase>("InventoryItemDatabase");
            if (inventoryItemDatabase)
            {
                var item = inventoryItemDatabase.GetItemById(pickupableItem.ItemId);
                if (item)
                {
                    AddItem(item);
                    Destroy(pickupableItem.gameObject);
                }
                else
                {
                    var temp = Resources.Load<ItemsDatabase>("ItemsDatabase").GetItemById<InventoryItem>(pickupableItem.ItemId);
                    if (temp)
                    {
                        AddItem(temp);
                    }
                    else
                    {
                        InventoryItem newItem = inventoryItemDatabase.CreateItem(pickupableItem.ItemId, pickupableItem.DisplayName, pickupableItem.Icon,
                            pickupableItem.Prefab, pickupableItem.Category, pickupableItem.IsStackable, pickupableItem.MaxStackSize, pickupableItem.Description);
                        AddItem(newItem);
                    }
                    Destroy(pickupableItem.gameObject);
                }
            }
        }
        
        public void AddItemToInventory(string itemId)
        {
            var itemDatabase = Resources.Load<InventoryItemDatabase>("InventoryItemDatabase");
            if (itemDatabase)
            {
                var item = itemDatabase.GetItemById(itemId);
                if (item)
                {
                    AddItem(item);
                }
            }
        }
        
        public bool HasItem(string itemId)
        {
            return FindSlotWithItem(itemId);
        }
        
        public void RemoveItem(string itemId)
        {
            var slot = FindSlotWithItem(itemId);
            if (slot)
            {
                slot.RemoveFromStack();
            }
        }
        
        public void RemoveEmptySlot(string itemId)
        {
            var slot = FindSlotWithItem(itemId);
            if (slot)
            {
                _slots.Remove(slot);
                Destroy(slot.gameObject);
            }
        }
    }
    
    [System.Serializable]
    public class InventoryCategory
    {
        public ToolCategory category;
        public string displayName;
        public Sprite categoryIcon;
    }
} 