using System.Collections.Generic;
using UnityEngine;

public class PlantBed : MonoBehaviour
{
    public List<InventoryData> validSeeds; // Seeds that can be planted here

    public bool IsSeedValid(InventoryData seed)
    {
        return validSeeds.Contains(seed);
    }
}
