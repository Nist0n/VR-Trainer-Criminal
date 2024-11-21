using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private Button startButton;
    
        [SerializeField] private Button exitButton;
        
        [SerializeField] private Button trainingButton;
        
        void Start()
        {
            startButton.onClick.AddListener(StartDoc);
            exitButton.onClick.AddListener(Exit);
            trainingButton.onClick.AddListener(StartTraining);
        }

        private void StartDoc()
        {
            Debug.Log("Doc Started");
        }

        private void StartTraining()
        {
            SceneManager.LoadScene("TrainingScene");
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
