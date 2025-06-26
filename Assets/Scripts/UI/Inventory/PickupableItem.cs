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
        
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public ToolCategory Category => category;
        public bool IsStackable => isStackable;
        public int MaxStackSize => maxStackSize;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer != null && originalMaterial == null)
            {
                originalMaterial = _renderer.material;
            }
            
            // Создаем подсказку если нужно
            if (showPickupPrompt && pickupPrompt == null)
            {
                CreatePickupPrompt();
            }
            
            // Скрываем подсказку изначально
            if (pickupPrompt != null)
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
            // Проверяем расстояние до игрока
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                // Ищем по камере если нет тега Player
                player = Camera.main?.gameObject;
            }
            
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
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
            if (showPickupPrompt && pickupPrompt != null)
            {
                pickupPrompt.SetActive(true);
            }
            
            Highlight();
        }
        
        private void OnPlayerExit()
        {
            if (showPickupPrompt && pickupPrompt != null)
            {
                pickupPrompt.SetActive(false);
            }
            
            Unhighlight();
        }
        
        public void Highlight()
        {
            if (_isHighlighted) return;
            
            _isHighlighted = true;
            
            if (_renderer != null && highlightMaterial != null)
            {
                _renderer.material = highlightMaterial;
            }
            
            onHighlight?.Invoke();
        }
        
        public void Unhighlight()
        {
            if (!_isHighlighted) return;
            
            _isHighlighted = false;
            
            if (_renderer != null && originalMaterial != null)
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
            
            // Вызываем событие
            onPickup?.Invoke();
            
            // Добавляем в инвентарь
            var inventory = FindAnyObjectByType<AdaptiveGridInventory>();
            if (inventory != null)
            {
                inventory.AddItemToInventory(itemId);
            }
            
            // Уничтожаем объект
            Destroy(gameObject);
        }
        
        private void CreatePickupPrompt()
        {
            // Создаем простую подсказку
            GameObject prompt = new GameObject("PickupPrompt");
            prompt.transform.SetParent(transform);
            
            // Добавляем Canvas для UI
            Canvas canvas = prompt.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            
            // Добавляем Canvas Scaler
            CanvasScaler scaler = prompt.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Добавляем Graphic Raycaster
            prompt.AddComponent<GraphicRaycaster>();
            
            // Создаем фон
            GameObject background = new GameObject("Background");
            background.transform.SetParent(prompt.transform);
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform bgRect = background.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(200, 50);
            bgRect.anchoredPosition = new Vector2(0, 1.5f);
            
            // Создаем текст
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
        
        // Методы для настройки в инспекторе
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
        
        // Автоматическое определение настроек на основе имени объекта
        [ContextMenu("Auto Setup From Name")]
        public void AutoSetupFromName()
        {
            string objName = gameObject.name.ToLower();
            
            // Определяем категорию
            if (objName.Contains("brush") || objName.Contains("scotch") || objName.Contains("tape"))
            {
                category = ToolCategory.Sampling;
            }
            else if (objName.Contains("microscope") || objName.Contains("magnifier") || objName.Contains("lens"))
            {
                category = ToolCategory.Analysis;
            }
            else if (objName.Contains("camera") || objName.Contains("photo") || objName.Contains("notebook"))
            {
                category = ToolCategory.Documentation;
            }
            else
            {
                category = ToolCategory.General;
            }
            
            // Устанавливаем ID и имя
            itemId = gameObject.name;
            displayName = gameObject.name;
            
            Debug.Log($"Auto setup completed for {gameObject.name}: Category = {category}");
        }
        
        // Визуализация в редакторе
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