using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Data;
using Items;
using TMPro;
using UnityEngine.Serialization;

namespace UI.Inventory
{
    public class InventoryContextMenu : MonoBehaviour
    {
        [FormerlySerializedAs("itemNameText")]
        [Header("UI Components")]
        [SerializeField]
        public TextMeshProUGUI ItemNameText;
        public TextMeshProUGUI DescriptionText;
        public Transform ButtonsContainer;
        public Button ActionButtonPrefab;
        
        [Header("Actions")]
        private Transform _spawnpoint;
        private InventoryItem _item;
        private InventorySlot _slot;
        private List<Button> _actionButtons = new List<Button>();
        
        public void Initialize(InventoryItem item, InventorySlot slot, Transform spawnpoint)
        {
            _item = item;
            _slot = slot;
            _spawnpoint = spawnpoint;
            
            UpdateUI();
            CreateActionButtons();
        }
        
        private void UpdateUI()
        {
            if (_item)
            {
                ItemNameText.text = _item.displayName;
                DescriptionText.text = _item.description;
            }
        }
        
        private void CreateActionButtons()
        {
            foreach (var button in _actionButtons)
            {
                if (button)
                    Destroy(button.gameObject);
            }
            _actionButtons.Clear();
            
            foreach (var actionType in _item.availableActions)
            {
                InventoryAction action = CreateActionFromType(actionType);
                if (action != null && action.CanExecute(_item))
                {
                    Button button = Instantiate(ActionButtonPrefab, ButtonsContainer);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = action.actionName;
                    button.onClick.AddListener(() => ExecuteAction(action));
                    _actionButtons.Add(button);
                }
            }
        }
        
        private InventoryAction CreateActionFromType(ItemActionType actionType)
        {
            switch (actionType)
            {
                case ItemActionType.Take:
                    return new TakeItemAction(_spawnpoint);
                case ItemActionType.Analysis:
                    return new AnalyzeItemAction();
                case ItemActionType.Discover:
                    return new DiscoverItemAction();
                default:
                    return null;
            }
        }
        
        private void ExecuteAction(InventoryAction action)
        {
            action.Execute(_item, _slot);
            Destroy(gameObject);
        }
    }
    
    [System.Serializable]
    public class InventoryAction
    {
        public string actionName;
        public string actionDescription;
        
        public virtual bool CanExecute(InventoryItem item)
        {
            return item;
        }
        
        public virtual void Execute(InventoryItem item, InventorySlot slot)
        {
        }
    }
    
    [System.Serializable]
    public class TakeItemAction : InventoryAction
    {
        private Transform _spawnpoint;
        public TakeItemAction(Transform spawnpoint)
        {
            _spawnpoint = spawnpoint;
            actionName = "Взять";
            actionDescription = "Достать предмет из инвентаря";
        }
        
        public override void Execute(InventoryItem item, InventorySlot slot)
        {
            if (item.prefab)
            {
                item.prefab.GetComponent<PickupableItem>().SetItemID(item.itemId);
                item.prefab.GetComponent<PickupableItem>().SetSurfaceName(item.surfaceName);
                item.prefab.GetComponent<PickupableItem>().SetTimeOfPhoto(item.timeOfPhoto);
                GameObject spawnedItem = Object.Instantiate(item.prefab, _spawnpoint.position, Quaternion.identity);
                
                Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = false;
                }
                
                slot.RemoveFromStack();
            }
        }
    }
    
    [System.Serializable]
    public class AnalyzeItemAction : InventoryAction
    {
        public AnalyzeItemAction()
        {
            actionName = "Отправить в лабораторию";
            actionDescription = "Распознание владельца";
        }
        
        public override void Execute(InventoryItem item, InventorySlot slot)
        {
            InventoryItem discoveredItem = item.CreateCopy();
            discoveredItem.RevealHiddenData();
            discoveredItem.description += $" - {EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(discoveredItem.itemId).OwnerName}";
            
            slot.SetItem(discoveredItem, new RectTransform());
        }
    }
    
    [System.Serializable]
    public class DiscoverItemAction : InventoryAction
    {
        public DiscoverItemAction()
        {
            actionName = "Отправить в лабораторию";
            actionDescription = "Распознание следа";
        }
        
        public override bool CanExecute(InventoryItem item)
        {
            return item;
        }
        
        public override void Execute(InventoryItem item, InventorySlot slot)
        {
            InventoryItem discoveredItem = item.CreateCopy();
            discoveredItem.RevealHiddenData();
            
            slot.SetItem(discoveredItem, new RectTransform());
        }
    }
} 