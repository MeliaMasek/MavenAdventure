using UnityEngine;

public class PlanterInteraction : MonoBehaviour
{
    private PlantManager plantManager;

    private void Start()
    {
        plantManager = FindObjectOfType<PlantManager>();
    }

    private void OnMouseDown()
    {
        if (plantManager != null)
        {
            plantManager.PlantSeedAt(transform); // Use this planter as the planting location
        }
    }
}