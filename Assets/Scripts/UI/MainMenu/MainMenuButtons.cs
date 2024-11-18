using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button startButton;
    
        [SerializeField] private Button exitButton;
        
        void Start()
        {
            startButton.onClick.AddListener(StartTraining);
            exitButton.onClick.AddListener(Exit);
        }

        private void StartTraining()
        {
            Debug.Log("Training Started");
        }
        
        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

    }
}
