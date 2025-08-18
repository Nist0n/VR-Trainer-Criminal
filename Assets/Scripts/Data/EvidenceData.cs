using UnityEngine;

namespace Data
{
    public abstract class EvidenceData : ScriptableObject
    {
        public string EvidenceId;
        public bool IsDiscovered;
        public Vector3 OriginalPosition; // Опционально
    }
} 