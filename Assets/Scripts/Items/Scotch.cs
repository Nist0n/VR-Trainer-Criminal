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
                    item.prefab.GetComponent<PickupableFingerprint>().SurfaceName = other.GetComponent<Fingerprint>().SurfaceName;
                    item.prefab.GetComponent<PickupableFingerprint>().TimeOfPhoto = other.GetComponent<Fingerprint>().TimeOfPhoto;
                    item.prefab.GetComponent<PickupableFingerprint>().ItemID(other.GetComponent<Fingerprint>().FingerprintId);
                    item.itemId = other.GetComponent<Fingerprint>().FingerprintId;
                    other.GetComponent<Fingerprint>().DeActivate();
                    _inventory.AddItem(item);
                    Debug.Log(item.itemId + " Поставленный ID");
                }
            }
        }
    }
}
