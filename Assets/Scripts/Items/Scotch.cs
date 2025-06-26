using System;
using UnityEngine;

namespace UI.Inventory
{
    public class Scotch : MonoBehaviour
    {
        [SerializeField] private AdaptiveGridInventory inventory;

        [SerializeField] private GameObject otpechatok;

        // Весь скрипт переделать, id не должен браться из префаба
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fingerprint"))
            {
                var itemDatabase = Resources.Load<InventoryItemDatabase>("InventoryItemDatabase");
                if (itemDatabase != null)
                {
                    Debug.Log("Database");
                    var item = itemDatabase.GetItemById(otpechatok.GetComponent<PickupableItem>().ItemId);
                    otpechatok.GetComponentInChildren<SpriteRenderer>().sprite =
                        other.gameObject.GetComponent<SpriteRenderer>().sprite;
                    other.gameObject.SetActive(false);
                    inventory.AddItem(item);
                }
            }
        }
    }
}
