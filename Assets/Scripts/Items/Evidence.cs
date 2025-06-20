using UnityEngine;

namespace Items
{
    public abstract class Evidence : MonoBehaviour
    {
        [SerializeField] protected GameObject evidenceObject;
        
        [SerializeField] protected string evidenceId;

        protected EvidenceData Data;

        protected virtual void Awake()
        {
            LoadEvidenceData();
            if (Data != null)
            {
                evidenceObject.SetActive(false);
            }
        }

        protected abstract void LoadEvidenceData();

        public virtual void Activate()
        {
            if (Data == null) return;
            
            evidenceObject.SetActive(true);
            
            EvidenceDatabase.Instance.DiscoverEvidence(evidenceId);
        }

        public virtual void DeActivate()
        {
            evidenceObject.SetActive(false);
        }

        public EvidenceData GetEvidenceData()
        {
            return Data;
        }
    }
} 