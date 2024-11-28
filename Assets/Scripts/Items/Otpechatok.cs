using UnityEngine;

namespace Items
{
    public class Otpechatok : MonoBehaviour
    {
        [SerializeField] private GameObject pechatka;
        
        public void Activate()
        {
            pechatka.SetActive(true);
        }
    
        public void DeActivate()
        {
            pechatka.SetActive(false);
        }
    }
}
