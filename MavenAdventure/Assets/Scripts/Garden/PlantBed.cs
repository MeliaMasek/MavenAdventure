using System.Collections.Generic;
using UnityEngine;
//Code created, borrowed, and modified from Chat GPT

public class PlantBed : MonoBehaviour
{
    public List<InventoryData> validSeeds; // Seeds that can be planted here

    public bool IsSeedValid(InventoryData seed)
    {
        return validSeeds.Contains(seed);
    }
}
