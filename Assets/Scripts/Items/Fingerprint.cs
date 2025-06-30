using System;
using UnityEngine;

namespace Items
{
    public class Fingerprint : Evidence
    {
        public string FingerprintId;
        
        protected override void Awake()
        {
            LoadEvidenceData();
            if (Data)
            {
                evidenceObject.SetActive(false);
            }
        }

        protected override void LoadEvidenceData()
        {
            Data = EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(evidenceId);
            
            if (Data == null)
            {
                Debug.LogError($"Fingerprint with ID {evidenceId} not found in database!");
            }
        }
        
        public override void Activate()
        {
            if (Data == null) return;
            
            evidenceObject.SetActive(true);
            
            EvidenceDatabase.Instance.DiscoverEvidence(evidenceId);
        }

        public override void DeActivate()
        {
            Destroy(evidenceObject);
        }
    }
}
