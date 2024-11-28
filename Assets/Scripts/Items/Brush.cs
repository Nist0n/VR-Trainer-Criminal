using System;
using UnityEngine;

namespace Items
{
    public class Brush : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Glass"))
            {
                Debug.Log("Glass");
                if (other.gameObject.GetComponent<Otpechatok>())
                {
                    Debug.Log("Acrive");
                    other.gameObject.GetComponent<Otpechatok>().Activate();
                }
            }
        }
    }
}
