using UnityEngine;

namespace Items
{
    public class Fingerprint : MonoBehaviour
    {
        [SerializeField] private GameObject fingerprintObject;
        
        public void Activate()
        {
            fingerprintObject.SetActive(true);
        }
    
        public void DeActivate()
        {
            fingerprintObject.SetActive(false);
        }
    }
}
