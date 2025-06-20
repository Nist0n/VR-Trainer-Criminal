using UnityEngine;

namespace Systems
{
    public class DistanceDemo : MonoBehaviour
    {
        public GameObject object1;
        public GameObject object2;
        public DistanceCalculator distanceCalculator;
        public SelectionManager selectionManager;
    
        void Update()
        {
            if (object1 != null && object2 != null)
            {
                // Выделяем объекты
                selectionManager.ClearAllHighlights();
                selectionManager.HighlightObject(object1, true);
                selectionManager.HighlightObject(object2, true);
            
                // Рассчитываем расстояние
                float distance = distanceCalculator.CalculateShortestDistance(object1, object2);
                Debug.Log($"Shortest distance between objects: {distance}");
            }
        }
    }
}
