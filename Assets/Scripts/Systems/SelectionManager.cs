using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class SelectionManager : MonoBehaviour
    {
        [Header("Highlight Settings")]
        public Color outlineColor = new Color(1f, 0.5f, 0f, 1f);
        public float outlineWidth = 0.03f;
        public bool useGPUInstancing = true;
    
        private List<Renderer> highlightedRenderers = new List<Renderer>();
        private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
        private Material outlineMaterial;
    
        private void Awake()
        {
            // Создаем материал из шейдера
            outlineMaterial = new Material(Shader.Find("Custom/URPOutline"));
            outlineMaterial.enableInstancing = useGPUInstancing;
        }

        public void HighlightObject(GameObject obj, bool highlight)
        {
            if (obj == null) return;
        
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
            foreach (Renderer renderer in renderers)
            {
                if (highlight)
                {
                    if (!highlightedRenderers.Contains(renderer))
                    {
                        // Сохраняем оригинальные материалы
                        originalMaterials[renderer] = renderer.sharedMaterials;
                        highlightedRenderers.Add(renderer);
                    
                        // Применяем outline материал
                        ApplyOutlineMaterial(renderer);
                    }
                }
                else
                {
                    if (highlightedRenderers.Contains(renderer))
                    {
                        // Восстанавливаем оригинальные материалы
                        if (originalMaterials.TryGetValue(renderer, out Material[] mats))
                        {
                            renderer.sharedMaterials = mats;
                            originalMaterials.Remove(renderer);
                        }
                        highlightedRenderers.Remove(renderer);
                    }
                }
            }
        }
    
        private void ApplyOutlineMaterial(Renderer renderer)
        {
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
        
            for (int i = 0; i < newMaterials.Length; i++)
            {
                Material outlineMat = new Material(outlineMaterial);
            
                // Копируем основные свойства из оригинального материала
                outlineMat.CopyPropertiesFromMaterial(renderer.sharedMaterials[i]);
            
                // Устанавливаем параметры outline
                outlineMat.SetColor("_OutlineColor", outlineColor);
                outlineMat.SetFloat("_OutlineWidth", outlineWidth);
            
                newMaterials[i] = outlineMat;
            }
        
            renderer.sharedMaterials = newMaterials;
        }
    
        public void ClearAllHighlights()
        {
            // Создаем копию списка для безопасной итерации
            var renderersToClear = new List<Renderer>(highlightedRenderers);
        
            foreach (Renderer renderer in renderersToClear)
            {
                if (renderer != null && originalMaterials.TryGetValue(renderer, out Material[] mats))
                {
                    renderer.sharedMaterials = mats;
                }
            }
        
            highlightedRenderers.Clear();
            originalMaterials.Clear();
        }
    
        private void OnDestroy()
        {
            ClearAllHighlights();
            if (outlineMaterial != null)
            {
                Destroy(outlineMaterial);
            }
        }
    }
}