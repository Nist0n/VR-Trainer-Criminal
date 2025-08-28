using System;
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
        [SerializeField] protected string itemId;
        [SerializeField] protected string displayName;
        [SerializeField] protected ToolCategory category = ToolCategory.Tools;
        [SerializeField] protected bool isStackable = false;
        [SerializeField] protected int maxStackSize = 1;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected string description;
        [SerializeField] protected string surfaceName;
        
        [Header("Visual Feedback")]
        [SerializeField] protected bool showPickupPrompt = true;
        [SerializeField] private GameObject pickupPrompt;
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material originalMaterial;
        
        [Header("Interaction")]
        [SerializeField] protected float interactionDistance = 2f;
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
        private DateTime? _timeOfPhoto;
        
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public ToolCategory Category => category;
        public bool IsStackable => isStackable;
        public int MaxStackSize => maxStackSize;
        public GameObject Prefab => prefab;
        public Sprite Icon => icon;
        public string Description => description;
        public string SurfaceName => surfaceName;
        public DateTime? TimeOfPhoto => _timeOfPhoto;

        public void SetItemID(string id) => itemId = id;
        public void SetTimeOfPhoto(DateTime? time) => _timeOfPhoto = time;
        public void SetSurfaceName(string nameOfSurface) => surfaceName = nameOfSurface;
        
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _renderer = GetComponent<Renderer>();
            if (_renderer && !originalMaterial)
            {
                originalMaterial = _renderer.material;
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