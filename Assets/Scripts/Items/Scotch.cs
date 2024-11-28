using System;
using UnityEngine;

namespace UI.Inventory
{
    public class Scotch : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;

        [SerializeField] private GameObject otpechatok;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Otpechatok"))
            {
                otpechatok.GetComponentInChildren<SpriteRenderer>().sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
                other.gameObject.SetActive(false);
                GameObject temp = Instantiate(otpechatok, inventory.transform);
                inventory.CheckEmpty().InsertItem(temp);
            }
        }
    }
}
