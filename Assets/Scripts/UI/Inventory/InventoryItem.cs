using UnityEngine;

namespace UI.Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string displayName;
        public Sprite icon;
        public GameObject prefab;
        public ToolCategory category;
        public Vector3 spawnOffset = Vector3.zero;
        public Quaternion spawnRotation = Quaternion.identity;
        public bool isStackable = false;
        public int maxStackSize = 1;
        
        [TextArea(2, 4)]
        public string description;
    }
    
    public enum ToolCategory
    {
        Sampling,      // Кисти, скотч, пинцеты
        Analysis,      // Микроскопы, лупы
        Documentation, // Фотоаппараты, блокноты
        General        // Общие инструменты
    }
} 