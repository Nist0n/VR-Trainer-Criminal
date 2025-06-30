using System;
using UnityEngine;

namespace Items
{
    public class Brush : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Fingerprint"))
            {
                Fingerprint fingerprint = other.gameObject.GetComponent<Fingerprint>();
                
                if (fingerprint)
                {
                    FingerprintData data = fingerprint.GetEvidenceData() as FingerprintData;
                    
                    if (data && !data.IsDiscovered)
                    {
                        fingerprint.Activate();
                        Debug.Log($"Found fingerprint belonging to: {data.OwnerName}");
                    }
                }
            }
        }
    }
}
