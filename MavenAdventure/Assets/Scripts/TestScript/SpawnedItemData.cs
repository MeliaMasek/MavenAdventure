using System;
using UnityEngine;

[Serializable]
public class SpawnedItemData
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;

    public SpawnedItemData(string prefabName, Vector3 position, Quaternion rotation)
    {
        this.prefabName = prefabName;
        this.position = position;
        this.rotation = rotation;
    }
}