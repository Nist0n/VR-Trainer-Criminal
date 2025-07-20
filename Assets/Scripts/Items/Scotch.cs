using System;
using Data;
using UI.Inventory;
using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
    public class Scotch : MonoBehaviour
    {
        private AdaptiveGridInventory _inventory;

        private void Start()
        {
            _inventory = FindAnyObjectByType<AdaptiveGridInventory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fingerprint"))
            {
                var itemsDatabase = Resources.Load<ItemsDatabase>("ItemsDatabase");
                if (itemsDatabase)
                {
                    var item = itemsDatabase.GetItemById<InventoryItem>(other.GetComponent<Fingerprint>().FingerprintId);
                    item.surfaceName = other.GetComponent<Fingerprint>().SurfaceName;
                    item.timeOfPhoto = other.GetComponent<Fingerprint>().TimeOfPhoto;
                    item.itemId = other.GetComponent<Fingerprint>().FingerprintId;
                    other.GetComponent<Fingerprint>().DeActivate();
                    _inventory.AddItem(item);
                }
            }
        }
    }
}
