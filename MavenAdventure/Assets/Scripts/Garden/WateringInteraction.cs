using UnityEngine;

public class WateringInteraction : MonoBehaviour
{
    public bool isWateringMode = false; // Tracks if watering mode is active
    public LayerMask plantLayer;       // Layer for plant objects
    public PlantManager plantManager; // Reference to the PlantManager script
    public Material dirtWet;          // Material for wet dirt
    public Material dirtDry;          // Material for dry dirt
    
    // Called when the Watering Button is clicked
    public void ActivateWateringMode()
    {
        isWateringMode = true;
        Debug.Log("Watering mode activated. Click a plant to water.");
    }

    private void Update()
    {
        if (isWateringMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer))
            {
                // Find the plant associated with the clicked GameObject
                foreach (var plant in plantManager.plants) // Access plants from PlantManager
                {
                    if (plant.baseStage == hit.collider.gameObject ||
                        plant.seedStage == hit.collider.gameObject ||
                        plant.sproutStage == hit.collider.gameObject ||
                        plant.matureStage == hit.collider.gameObject)
                    {
                        plant.isWatered = true;
                        ChangeDirtMaterial(plant.baseStage); // Change the dirt material when watered
                        Debug.Log($"Watered the plant: {hit.collider.gameObject.name}");
                        isWateringMode = false;
                        break;
                    }
                }
            }
        }
    }
    
    private void ChangeDirtMaterial(GameObject baseStage)
    {
        // Ensure the baseStage has at least two children
        if (baseStage.transform.childCount > 1)
        {
            // Get the second child (index 1, as indexing is zero-based)
            Transform secondChild = baseStage.transform.GetChild(1);

            // Attempt to get the MeshRenderer from the second child
            MeshRenderer meshRenderer = secondChild.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.material = dirtWet; // Change the material to the wet material
                Debug.Log($"Changed material to wet for second child of {baseStage.name}.");
            }
            else
            {
                Debug.LogWarning($"Second child of {baseStage.name} does not have a MeshRenderer.");
            }
        }
        else
        {
            Debug.LogWarning($"{baseStage.name} does not have a second child.");
        }
    }
}
