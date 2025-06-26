using System.Collections.Generic;
using UnityEngine;

namespace UI.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItemDatabase", menuName = "Inventory/Item Database")]
    public class InventoryItemDatabase : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> allItems = new List<InventoryItem>();
        
        private Dictionary<string, InventoryItem> _itemLookup = new Dictionary<string, InventoryItem>();
        private Dictionary<ToolCategory, List<InventoryItem>> _categoryLookup = new Dictionary<ToolCategory, List<InventoryItem>>();
        
        private void OnEnable()
        {
            BuildLookupTables();
        }
        
        private void BuildLookupTables()
        {
            _itemLookup.Clear();
            _categoryLookup.Clear();
            
            foreach (var item in allItems)
            {
                // Добавляем в общий lookup
                if (!string.IsNullOrEmpty(item.itemId))
                {
                    _itemLookup[item.itemId] = item;
                }
                
                // Добавляем в категории
                if (!_categoryLookup.ContainsKey(item.category))
                {
                    _categoryLookup[item.category] = new List<InventoryItem>();
                }
                _categoryLookup[item.category].Add(item);
            }
        }
        
        public InventoryItem GetItemById(string itemId)
        {
            return _itemLookup.TryGetValue(itemId, out var item) ? item : null;
        }
        
        public List<InventoryItem> GetItemsByCategory(ToolCategory category)
        {
            return _categoryLookup.TryGetValue(category, out var items) ? items : new List<InventoryItem>();
        }
        
        public List<InventoryItem> GetAllItems()
        {
            return new List<InventoryItem>(allItems);
        }
        
        public void AddItem(InventoryItem item)
        {
            if (item != null && !allItems.Contains(item))
            {
                allItems.Add(item);
                BuildLookupTables();
            }
        }
        
        public void RemoveItem(InventoryItem item)
        {
            if (allItems.Remove(item))
            {
                BuildLookupTables();
            }
        }
        
        public void RemoveItemById(string itemId)
        {
            var item = GetItemById(itemId);
            if (item != null)
            {
                RemoveItem(item);
            }
        }
        
        // Методы для создания предметов программно
        public InventoryItem CreateItem(string itemId, string displayName, Sprite icon, GameObject prefab, ToolCategory category)
        {
            var item = new InventoryItem
            {
                itemId = itemId,
                displayName = displayName,
                icon = icon,
                prefab = prefab,
                category = category
            };
            
            AddItem(item);
            return item;
        }
        
        // Методы для сохранения/загрузки данных
        public void SaveToPlayerPrefs()
        {
            // Сохраняем состояние инвентаря (какие предметы у игрока)
            // Это можно расширить для сохранения прогресса
        }
        
        public void LoadFromPlayerPrefs()
        {
            // Загружаем состояние инвентаря
        }
    }
    
    // Расширение для работы с конкретными типами инструментов
    public static class InventoryItemExtensions
    {
        public static bool IsSamplingTool(this InventoryItem item)
        {
            return item.category == ToolCategory.Sampling;
        }
        
        public static bool IsAnalysisTool(this InventoryItem item)
        {
            return item.category == ToolCategory.Analysis;
        }
        
        public static bool IsDocumentationTool(this InventoryItem item)
        {
            return item.category == ToolCategory.Documentation;
        }
        
        public static bool IsGeneralTool(this InventoryItem item)
        {
            return item.category == ToolCategory.General;
        }
        
        public static string GetCategoryDisplayName(this ToolCategory category)
        {
            return category switch
            {
                ToolCategory.Sampling => "Сбор образцов",
                ToolCategory.Analysis => "Анализ",
                ToolCategory.Documentation => "Документирование",
                ToolCategory.General => "Общие",
                _ => "Неизвестно"
            };
        }
    }
} 