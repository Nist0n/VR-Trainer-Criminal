using System;
using Data;
using UI.Inventory;
using UnityEngine;

namespace Items
{
    public class Fingerprint : Evidence
    {
        public string FingerprintId;
        public string SurfaceName = "Не указано";
        public DateTime? TimeOfPhoto;
        [SerializeField] private string assignedSurfaceName;
        private bool _pendingDestroy = false;
        
        protected override void Awake()
        {
            LoadEvidenceData();
            if (Data)
            {
                evidenceObject.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (!_pendingDestroy || !evidenceObject) return;
            Destroy(evidenceObject);
            Destroy(gameObject);
            _pendingDestroy = false;
        }

        protected override void LoadEvidenceData()
        {
            Data = EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(evidenceId);
            
            if (!Data)
            {
                Debug.LogError($"Fingerprint with ID {evidenceId} not found in database!");
            }
        }
        
        public override void Activate()
        {
            if (!Data) return;
            
            evidenceObject.SetActive(true);
            
            EvidenceDatabase.Instance.DiscoverEvidence(evidenceId);
        }
        
        public void FixatePhoto()
        {
            if (!TimeOfPhoto.HasValue)
            {
                EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(evidenceId).TimeOfPhoto = DateTime.Now;
                TimeOfPhoto = DateTime.Now;
                SurfaceName = assignedSurfaceName;
                Debug.Log($"Отпечаток зафиксирован: {SurfaceName} в {TimeOfPhoto}");
            }
        }

        public override void DeActivate(InventoryItem item, AdaptiveGridInventory inventory)
        {
            item.surfaceName = SurfaceName;
            item.timeOfPhoto = TimeOfPhoto;
            item.itemId = FingerprintId;
            inventory.AddItem(item);
            Debug.Log(item.timeOfPhoto.Value);
            evidenceObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
