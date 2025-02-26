using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    public ProduceData produceData; // Reference to the produce data

    public ProduceData GetProduceData()
    {
        return produceData;
    }
}
