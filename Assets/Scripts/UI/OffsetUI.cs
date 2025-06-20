using UnityEngine;

namespace UI
{
    public class OffsetUI : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_camera) return;
            
            gameObject.transform.rotation = _camera.transform.rotation;

            gameObject.transform.position = _camera.transform.position;
        }
    }
}
