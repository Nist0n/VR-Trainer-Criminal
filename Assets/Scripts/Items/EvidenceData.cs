using UnityEngine;

namespace Items
{
    public abstract class EvidenceData : ScriptableObject
    {
        public string EvidenceId;
        public string OwnerName; // Опционально
        public bool IsDiscovered;
        public Vector3 OriginalPosition; // Опционально
    }
} 