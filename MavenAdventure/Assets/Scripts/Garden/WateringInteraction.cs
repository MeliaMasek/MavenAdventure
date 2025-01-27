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
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");

                // Find the plant associated with the clicked GameObject
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  // Check if clicked object matches the current stage object
                    {
                        Debug.Log($"Watering plant: {hit.collider.gameObject.name}");

                        if (!plant.isWatered)
                        {
                            plant.isWatered = true;
                            ChangeDirtMaterial(plant.currentStageObject); // Change the dirt material when watered
                            Debug.Log($"Watered the plant: {hit.collider.gameObject.name}");
                        }
                        else
                        {
                            Debug.Log("This plant is already watered today.");
                        }

                        isWateringMode = false;
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }

    private void ChangeDirtMaterial(GameObject stageObject)
    {
        // Find all child objects with the "Dirt" tag
        Transform[] childTransforms = stageObject.GetComponentsInChildren<Transform>();

        bool materialChanged = false;

        foreach (Transform child in childTransforms)
        {
            if (child.CompareTag("Dirt"))
            {
                // Get the MeshRenderer of the dirt object
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = dirtWet; // Change to wet material
                    materialChanged = true;
                    Debug.Log($"Changed material to wet for {child.name} in {stageObject.name}.");
                }
                else
                {
                    Debug.LogWarning($"No MeshRenderer found on {child.name} tagged as 'Dirt'.");
                }
            }
        }

        if (!materialChanged)
        {
            Debug.LogWarning($"No objects tagged as 'Dirt' found in {stageObject.name}.");
        }
    }
}
