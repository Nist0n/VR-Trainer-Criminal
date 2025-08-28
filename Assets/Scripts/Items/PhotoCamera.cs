using System;
using UI.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Items
{
    public class PhotoCamera : MonoBehaviour
    {
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private XRGrabInteractable grabInteractable;
        [SerializeField] private InputActionProperty selectAction;

        private PhotoAlbum _photoAlbum;
        private bool _isHeld = false;
        private Camera _photoCamera;

        private void Awake()
        {
            _photoCamera = GetComponentInChildren<Camera>();
            _photoAlbum = FindAnyObjectByType<PhotoAlbum>();
            grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.AddListener(_ => _isHeld = true);
            grabInteractable.selectExited.AddListener(_ => _isHeld = false);
        }

        private void Update()
        {
            if (_isHeld && (Input.GetKeyDown(KeyCode.F) || selectAction.action.triggered))
            {
                TakePhoto();
            }
        }

        private void TakePhoto()
        {
            RenderTexture.active = renderTexture;
            Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            photo.Apply();
            RenderTexture.active = null;
            
            _photoAlbum.AddPhoto(photo, DateTime.Now);

            var fingerprints = FindObjectsByType<Fingerprint>(FindObjectsSortMode.None);
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_photoCamera);
            
            Debug.Log(fingerprints.Length + " Количество отпечатков");

            foreach (var fp in fingerprints)
            {
                var rend = fp.GetComponentInParent<Renderer>();
                Debug.Log(rend);
                if (rend && GeometryUtility.TestPlanesAABB(planes, rend.bounds))
                {
                    Debug.Log("Найден отпечаток");
                    fp.FixatePhoto();
                }
            }
        }
    }
}