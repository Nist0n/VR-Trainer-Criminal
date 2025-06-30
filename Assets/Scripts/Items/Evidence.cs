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
        }

        protected abstract void LoadEvidenceData();

        public virtual void Activate() {}

        public virtual void DeActivate() {}

        public EvidenceData GetEvidenceData()
        {
            return Data;
        }
    }
} 