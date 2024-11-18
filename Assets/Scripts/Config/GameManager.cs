using Audio;
using UnityEngine;

namespace Config
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public bool IsPaused = false;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            AudioManager.Instance.PlayMusic("Test");
        }
    }
}
