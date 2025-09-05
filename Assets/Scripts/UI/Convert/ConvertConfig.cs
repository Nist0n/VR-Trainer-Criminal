using System;
using Data;
using TMPro;
using UI.Inventory;
using UnityEngine;

namespace UI.Convert
{
    public class ConvertConfig : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI date;
        [SerializeField] private TextMeshProUGUI place;
        private PickupableItem _fingerprintObject;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Fingerprint"))
            {
                _fingerprintObject = other.gameObject.GetComponent<PickupableItem>();
                place.text = other.GetComponent<PickupableItem>().SurfaceName;
                var timeOfPhoto = EvidenceDatabase.Instance.GetEvidenceById<FingerprintData>(_fingerprintObject.ItemId).TimeOfPhoto;
                if (timeOfPhoto.HasValue)
                    date.text = timeOfPhoto.Value.ToString("yyyy-MM-dd HH:mm:ss");
                else
                {
                    date.text = "Не указана";
                }
                Debug.Log($"Время: {timeOfPhoto}");
                other.gameObject.SetActive(false);
            }
        }

        public PickupableItem ConvertFingerprint()
        {
            if (_fingerprintObject)
            {
                _fingerprintObject.SetDisplayName(_fingerprintObject.DisplayName + " в конверте");
                Destroy(gameObject);
                return _fingerprintObject;
            }
            Destroy(gameObject);
            return null;
        }
    }
}
