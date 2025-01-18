using UnityEngine;

public class SwapPrefabs : MonoBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;

    private GameObject currentPrefab;

    void Start()
    {
        // Set the initial prefab (for example, prefabA) when the game starts
        SetPrefab(prefabA);
    }

    public void PrefabSwap()
    {
        // Toggle between prefabs
        if (currentPrefab == prefabA)
        {
            SetPrefab(prefabB);
        }
        else
        {
            SetPrefab(prefabA);
        }
    }

    void SetPrefab(GameObject prefab)
    {
        // Destroy the current prefab instance (if any)
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }

        // Instantiate the new prefab and set it as the current one
        currentPrefab = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}