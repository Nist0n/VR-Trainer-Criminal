using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("UI Components")]
        public Image IconImage;
        public Image BackgroundImage;
        public TextMeshProUGUI CountText;
        [SerializeField] private GameObject contextMenuPrefab;
        
        [Header("Visual Settings")]
        [SerializeField] private Color emptyColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        [SerializeField] private Color filledColor = Color.white;
        [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 1f, 0.8f);
        
        [Header("Input")]
        [SerializeField] private InputActionProperty selectAction;

        private Transform _spawnpoint;
        private AdaptiveGridInventory _inventory;
        private InventoryItem _item;
        private int _itemCount = 0;
        private GameObject _contextMenu;
        private bool _isHovered = false;
        
        public InventoryItem Item => _item;
        public int ItemCount => _itemCount;
        public bool IsEmpty => _item == null;
        
        private void Start()
        {
            _inventory = GetComponentInParent<AdaptiveGridInventory>();
            UpdateVisuals();
        }
        
        private void Update()
        {
            if (_isHovered && selectAction.action.triggered)
            {
                ShowContextMenu();
            }
        }
        
        public void SetItem(InventoryItem item, Transform spawnpoint, int count = 1)
        {
            _spawnpoint = spawnpoint;
            _item = item;
            _itemCount = count;
            UpdateVisuals();
        }
        
        public void ClearSlot()
        {
            _inventory.RemoveEmptySlot(_item.itemId);
        }
        
        public void AddToStack(int amount = 1)
        {
            if (_item != null && _item.isStackable)
            {
                _itemCount = Mathf.Min(_itemCount + amount, _item.maxStackSize);
                UpdateVisuals();
            }
        }
        
        public bool RemoveFromStack(int amount = 1)
        {
            if (_item == null || _itemCount < amount) return false;
            
            _itemCount -= amount;
            if (_itemCount <= 0)
            {
                ClearSlot();
            }
            else
            {
                UpdateVisuals();
            }
            return true;
        }
        
        private void UpdateVisuals()
        {
            if (_item != null)
            {
                IconImage.sprite = _item.icon;
                IconImage.color = filledColor;
                BackgroundImage.color = filledColor;
                
                if (_item.isStackable && _itemCount > 1)
                {
                    CountText.text = _itemCount.ToString();
                    CountText.gameObject.SetActive(true);
                }
                else
                {
                    CountText.gameObject.SetActive(false);
                }
            }
            else
            {
                IconImage.sprite = null;
                IconImage.color = emptyColor;
                BackgroundImage.color = emptyColor;
                CountText.gameObject.SetActive(false);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            if (!IsEmpty)
            {
                BackgroundImage.color = hoverColor;
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            UpdateVisuals();
            HideContextMenu();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsEmpty)
            {
                ShowContextMenu();
            }
        }
        
        private void ShowContextMenu()
        {
            if (_contextMenu != null) return;
            
            _contextMenu = Instantiate(contextMenuPrefab, transform);
            var contextMenu = _contextMenu.GetComponent<InventoryContextMenu>();
            contextMenu.Initialize(_item, this, _spawnpoint);
            
            // Позиционируем меню рядом с ячейкой
            RectTransform rectTransform = _contextMenu.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width, 0);
        }
        
        private void HideContextMenu()
        {
            if (_contextMenu != null)
            {
                Destroy(_contextMenu);
                _contextMenu = null;
            }
        }
    }
} 