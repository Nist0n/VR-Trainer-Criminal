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
                Fingerprint fingerprint = other.gameObject.GetComponent<Fingerprint>();
                
                if (fingerprint != null)
                {
                    FingerprintData data = fingerprint.GetEvidenceData() as FingerprintData;
                    
                    if (data != null && !data.IsDiscovered)
                    {
                        fingerprint.Activate();
                        Debug.Log($"Found fingerprint belonging to: {data.OwnerName}");
                    }
                }
            }
        }
    }
}
