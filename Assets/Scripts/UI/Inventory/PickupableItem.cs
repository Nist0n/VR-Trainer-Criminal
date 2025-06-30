using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Inventory
{
    /// <summary>
    /// Компонент для предметов, которые можно подбирать в инвентарь
    /// </summary>
    public class PickupableItem : MonoBehaviour
    {
        [Header("Item Settings")]
        [SerializeField] private string itemId;
        [SerializeField] private string displayName;
        [SerializeField] private ToolCategory category = ToolCategory.General;
        [SerializeField] private bool isStackable = false;
        [SerializeField] private int maxStackSize = 1;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;
        [SerializeField] private string description;
        
        [Header("Visual Feedback")]
        [SerializeField] private bool showPickupPrompt = true;
        [SerializeField] private GameObject pickupPrompt;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material originalMaterial;
        
        [Header("Interaction")]
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private LayerMask playerLayer = -1;
        [SerializeField] private bool requirePlayerProximity = true;
        
        [Header("Events")]
        [SerializeField] private UnityEvent onPickup;
        [SerializeField] private UnityEvent onHighlight;
        [SerializeField] private UnityEvent onUnhighlight;
        
        private Renderer _renderer;
        private bool _isHighlighted = false;
        private bool _isPlayerNearby = false;
        private GameObject _player;
        
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public ToolCategory Category => category;
        public bool IsStackable => isStackable;
        public int MaxStackSize => maxStackSize;
        public GameObject Prefab => prefab;
        public Sprite Icon => icon;
        public string Description => description;
        
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _renderer = GetComponent<Renderer>();
            if (_renderer && !originalMaterial)
            {
                originalMaterial = _renderer.material;
            }
            
            if (showPickupPrompt && !pickupPrompt)
            {
                CreatePickupPrompt();
            }
            
            if (pickupPrompt)
            {
                pickupPrompt.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (requirePlayerProximity)
            {
                CheckPlayerProximity();
            }
        }
        
        private void CheckPlayerProximity()
        {
            if (_player)
            {
                float distance = Vector3.Distance(transform.position, _player.transform.position);
                bool wasNearby = _isPlayerNearby;
                _isPlayerNearby = distance <= interactionDistance;
                
                if (_isPlayerNearby != wasNearby)
                {
                    if (_isPlayerNearby)
                    {
                        OnPlayerEnter();
                    }
                    else
                    {
                        OnPlayerExit();
                    }
                }
            }
        }
        
        private void OnPlayerEnter()
        {
            if (showPickupPrompt && pickupPrompt)
            {
                pickupPrompt.SetActive(true);
            }
            
            Highlight();
        }
        
        private void OnPlayerExit()
        {
            if (showPickupPrompt && pickupPrompt)
            {
                pickupPrompt.SetActive(false);
            }
            
            Unhighlight();
        }
        
        public void Highlight()
        {
            if (_isHighlighted) return;
            
            _isHighlighted = true;
            
            if (_renderer && highlightMaterial)
            {
                _renderer.material = highlightMaterial;
            }
            
            onHighlight?.Invoke();
        }
        
        public void Unhighlight()
        {
            if (!_isHighlighted) return;
            
            _isHighlighted = false;
            
            if (_renderer && originalMaterial)
            {
                _renderer.material = originalMaterial;
            }
            
            onUnhighlight?.Invoke();
        }
        
        public bool CanBePickedUp()
        {
            if (requirePlayerProximity && !_isPlayerNearby)
                return false;
                
            return true;
        }
        
        public void Pickup()
        {
            if (!CanBePickedUp()) return;
            
            onPickup?.Invoke();
            
            var inventory = FindAnyObjectByType<AdaptiveGridInventory>();
            if (inventory)
            {
                inventory.AddItemToInventory(itemId);
            }
            
            Destroy(gameObject);
        }
        
        private void CreatePickupPrompt()
        {
            GameObject prompt = new GameObject("PickupPrompt");
            prompt.transform.SetParent(transform);
            
            Canvas canvas = prompt.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            
            CanvasScaler scaler = prompt.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            prompt.AddComponent<GraphicRaycaster>();
            
            GameObject background = new GameObject("Background");
            background.transform.SetParent(prompt.transform);
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(200, 50);
            bgRect.anchoredPosition = new Vector2(0, 1.5f);
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(prompt.transform);
            
            Text text = textObj.AddComponent<Text>();
            text.text = $"Нажмите E для подбора\n{displayName}";
            text.fontSize = 12;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(200, 50);
            textRect.anchoredPosition = new Vector2(0, 1.5f);
            
            pickupPrompt = prompt;
        }
        
        public void SetItemId(string newItemId)
        {
            itemId = newItemId;
        }
        
        public void SetDisplayName(string newDisplayName)
        {
            displayName = newDisplayName;
        }
        
        public void SetCategory(ToolCategory newCategory)
        {
            category = newCategory;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (requirePlayerProximity)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, interactionDistance);
            }
        }
    }
} 