using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

namespace UI.Training
{
    public class CubesGame : MonoBehaviour
    {
        [SerializeField] private NPCConversation endTraining;
    
        [SerializeField] private GameObject table;
    
        private bool _cubesGameStarted;
            
        private bool _trainingEnded;
    
        [SerializeField] private List<GameObject> _cubes = new List<GameObject>();
    
        public void CubesGameBool(bool cubes) => _cubesGameStarted = cubes;
    
        private void Update()
        {
            if (_cubes.Count >= 5 && !_trainingEnded && _cubesGameStarted)
            {
                _trainingEnded = true;
                table.SetActive(true);
                ConversationManager.Instance.StartConversation(endTraining);
            }
        }
    
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Cube") && _cubesGameStarted)
            {
                Debug.Log("Finded");
                foreach (var cube in _cubes)
                {
                    if (cube.name == other.gameObject.name)
                    {
                        return;
                    }
                }
                Debug.Log("Added");
                _cubes.Add(other.gameObject);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Cube") && _cubesGameStarted)
            {
                foreach (var cube in _cubes)
                {
                    if (cube.name == other.gameObject.name)
                    {
                        _cubes.Remove(cube);
                        return;
                    }
                }
            }
        }
    }
}
