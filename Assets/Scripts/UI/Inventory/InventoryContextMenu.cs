using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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
        [SerializeField]
        public List<InventoryAction> AvailableActions;

        private Transform _spawnpoint;
        private InventoryItem _item;
        private InventorySlot _slot;
        private List<Button> _actionButtons = new List<Button>();
        
        public void Initialize(InventoryItem item, InventorySlot slot, Transform spawnpoint)
        {
            _item = item;
            _slot = slot;

            _spawnpoint = spawnpoint;
            
            AvailableActions.Add(new TakeItemAction(_spawnpoint));
            
            UpdateUI();
            CreateActionButtons();
        }
        
        private void UpdateUI()
        {
            if (_item != null)
            {
                ItemNameText.text = _item.displayName;
                DescriptionText.text = _item.description;
            }
        }
        
        private void CreateActionButtons()
        {
            // Очищаем старые кнопки
            foreach (var button in _actionButtons)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }
            _actionButtons.Clear();
            
            // Создаем кнопки для доступных действий
            foreach (var action in AvailableActions)
            {
                if (action.CanExecute(_item))
                {
                    Button button = Instantiate(ActionButtonPrefab, ButtonsContainer);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = action.actionName;
                    button.onClick.AddListener(() => ExecuteAction(action));
                    _actionButtons.Add(button);
                }
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
            return item != null;
        }
        
        public virtual void Execute(InventoryItem item, InventorySlot slot)
        {
            // Базовая реализация - можно переопределить в наследниках
            Debug.Log($"Executing {actionName} on {item.displayName}");
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
            if (item.prefab != null)
            {
                GameObject spawnedItem = Object.Instantiate(item.prefab, _spawnpoint.position, Quaternion.identity);
                
                Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                
                slot.RemoveFromStack();
            }
        }
    }
    
    [System.Serializable]
    public class DropItemAction : InventoryAction
    {
        public DropItemAction()
        {
            actionName = "Выбросить";
            actionDescription = "Выбросить предмет из инвентаря";
        }
        
        public override void Execute(InventoryItem item, InventorySlot slot)
        {
            slot.ClearSlot();
        }
    }
    
    [System.Serializable]
    public class UseItemAction : InventoryAction
    {
        public UseItemAction()
        {
            actionName = "Использовать";
            actionDescription = "Использовать предмет";
        }
        
        public override bool CanExecute(InventoryItem item)
        {
            // Проверяем, можно ли использовать предмет
            return item != null && item.itemId.Contains("usable");
        }
        
        public override void Execute(InventoryItem item, InventorySlot slot)
        {
            // Логика использования предмета
            Debug.Log($"Using {item.displayName}");
        }
    }
} 