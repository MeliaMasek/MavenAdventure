using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour

{
    public List<Transform> spawnLocations; // Define spawn locations in the Unity Editor
    public GameObject[] itemPrefabs; // Pool of item prefabs
    public int numberOfItemsToSpawn = 1; // Number of items to spawn at once
    public float changeInterval = 10f; // Interval for changing the spawned item
    private float timer; // Timer for tracking when to change the spawned item

    private List<GameObject> spawnedItems = new List<GameObject>(); // List to keep track of spawned items
    private List<Transform> usedSpawnLocations = new List<Transform>(); // List to keep track of used spawn locations

    void Start()
    {
        SpawnRandomItem();
        timer = changeInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ChangeSpawnedItem();
            timer = changeInterval;
        }
    }

    void SpawnRandomItem()
    {
        if (spawnLocations.Count == 0 || itemPrefabs.Length == 0)
        {
            Debug.LogWarning("No spawn locations or item prefabs defined!");
            return;
        }

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            // Get available spawn locations
            List<Transform> availableSpawnLocations = new List<Transform>(spawnLocations);
            availableSpawnLocations.RemoveAll(loc => usedSpawnLocations.Contains(loc));

            if (availableSpawnLocations.Count == 0)
            {
                Debug.LogWarning("No available spawn locations!");
                return;
            }

            // Randomly select a spawn location from available ones
            int randomIndex = Random.Range(0, availableSpawnLocations.Count);
            Transform spawnPoint = availableSpawnLocations[randomIndex];

            // Randomly select an item from the pool
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // Spawn the item at the selected location
            GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

            // Add the spawned item to the list of spawned items
            spawnedItems.Add(spawnedItem);

            // Mark the spawn location as used
            usedSpawnLocations.Add(spawnPoint);
        }
    }

    void ChangeSpawnedItem()
    {
        // Destroy all existing spawned items
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear(); // Clear the list of spawned items
        usedSpawnLocations.Clear(); // Clear the list of used spawn locations

        // Spawn new random items
        SpawnRandomItem();
    }
}