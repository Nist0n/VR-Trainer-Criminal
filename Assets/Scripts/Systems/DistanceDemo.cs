using UnityEngine;

namespace Systems
{
    public class DistanceDemo : MonoBehaviour
    {
        [SerializeField] private DistanceCalculator distanceCalculator;
    
        public void CheckDistance(GameObject object1, GameObject object2)
        {
            if (object1 && object2)
            {
                float distance = distanceCalculator.CalculateShortestDistance(object1, object2);
                // Вывести эту информацию
            }
        }
    }
}
