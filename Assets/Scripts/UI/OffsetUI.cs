using UnityEngine;

namespace UI
{
    public class OffsetUI : MonoBehaviour
    {
        void Update()
        {
            gameObject.transform.rotation = Camera.main.transform.rotation;

            gameObject.transform.position = Camera.main.transform.position;
        }
    }
}
