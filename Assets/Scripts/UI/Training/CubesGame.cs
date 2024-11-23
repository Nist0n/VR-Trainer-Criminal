using System;
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

        private int _cubesCount;
    
        [SerializeField] private List<GameObject> _cubes = new List<GameObject>();
    
        public void CubesGameBool(bool cubes) => _cubesGameStarted = cubes;

        private void Start()
        {
            _cubesCount = GameObject.FindGameObjectsWithTag("Cube").Length;
        }

        private void Update()
        {
            if (_cubes.Count >= _cubesCount && !_trainingEnded && _cubesGameStarted)
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
                foreach (var cube in _cubes)
                {
                    if (cube.name == other.gameObject.name)
                    {
                        return;
                    }
                }
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
