using UnityEngine;

namespace Items
{
    public class Fingerprint : Evidence
    {
        protected override void LoadEvidenceData()
        {
            Data = EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(evidenceId);
            
            if (Data == null)
            {
                Debug.LogError($"Fingerprint with ID {evidenceId} not found in database!");
            }
        }
    }
}
