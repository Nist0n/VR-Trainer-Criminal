using UnityEngine;

namespace Systems.Omp
{
    /// <summary>
    /// Lightweight helper that can be hooked to UnityEvent / animation events to report OМП actions.
    /// </summary>
    public class OmpActionReporter : MonoBehaviour
    {
        [SerializeField] private string actionId;
        [SerializeField] private string toolIdOverride;

        public void Report()
        {
            ReportWithTool(toolIdOverride);
        }

        public void ReportWithTool(string toolId)
        {
            if (string.IsNullOrEmpty(actionId))
            {
                Debug.LogWarning($"{nameof(OmpActionReporter)}: actionId is not set.");
                return;
            }

            OmpActionAnalyzer analyzer = OmpActionAnalyzer.Instance;
            if (!analyzer)
            {
                Debug.LogWarning($"{nameof(OmpActionReporter)}: analyzer is not present in scene.");
                return;
            }

            OmpActionContext context = new OmpActionContext(toolId, transform.position, gameObject.name);
            analyzer.RecordAction(actionId, context);
        }
    }
}

