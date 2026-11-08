using UnityEngine;

[DisallowMultipleComponent]
public class RealityAppearance : MonoBehaviour
{
    [SerializeField] private GameObject[] realityAVisuals = new GameObject[0];
    [SerializeField] private GameObject[] realityBVisuals = new GameObject[0];
    [SerializeField] private RendererMaterialSet[] rendererMaterials = new RendererMaterialSet[0];

    [System.Serializable]
    private class RendererMaterialSet
    {
        public Renderer target;
        public Material[] realityAMaterials = new Material[0];
        public Material[] realityBMaterials = new Material[0];

        [System.NonSerialized] public Material[] originalMaterials;
    }

    private void Awake()
    {
        foreach (RendererMaterialSet materialSet in rendererMaterials)
        {
            if (materialSet.target != null)
            {
                materialSet.originalMaterials = materialSet.target.sharedMaterials;
            }
        }
    }

    private void OnEnable()
    {
        QuantumGoggles.RealityChanged += ApplyAppearance;
        ApplyAppearance(QuantumGoggles.IsRealityBActive);
    }

    private void OnDisable()
    {
        QuantumGoggles.RealityChanged -= ApplyAppearance;
    }

    private void ApplyAppearance(bool realityBActive)
    {
        SetVisuals(realityAVisuals, !realityBActive);
        SetVisuals(realityBVisuals, realityBActive);

        foreach (RendererMaterialSet materialSet in rendererMaterials)
        {
            if (materialSet.target == null)
            {
                continue;
            }

            Material[] selectedMaterials = realityBActive
                ? materialSet.realityBMaterials
                : materialSet.realityAMaterials;
            if (selectedMaterials.Length == 0)
            {
                selectedMaterials = materialSet.originalMaterials;
            }

            if (selectedMaterials != null)
            {
                materialSet.target.sharedMaterials = selectedMaterials;
            }
        }
    }

    private static void SetVisuals(GameObject[] visuals, bool active)
    {
        foreach (GameObject visual in visuals)
        {
            if (visual != null && visual.activeSelf != active)
            {
                visual.SetActive(active);
            }
        }
    }
}
