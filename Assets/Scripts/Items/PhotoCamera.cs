using System;
using UI.Inventory;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Items
{
    public class PhotoCamera : MonoBehaviour
    {
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private XRGrabInteractable grabInteractable;

        private PhotoAlbum _photoAlbum;
        private bool _isHeld = false;

        private void Awake()
        {
            _photoAlbum = FindAnyObjectByType<PhotoAlbum>();
            grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.AddListener(_ => _isHeld = true);
            grabInteractable.selectExited.AddListener(_ => _isHeld = false);
        }

        private void Update()
        {
            if (_isHeld && Input.GetKeyDown(KeyCode.F))
            {
                TakePhoto();
                Debug.Log("ФОТО");
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
        }
    }
}