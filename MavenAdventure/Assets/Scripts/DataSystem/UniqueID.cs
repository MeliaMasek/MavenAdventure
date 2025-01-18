using System;
using UnityEngine;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
//Code borrowed and Modified from ChatGPT

[System.Serializable]

public class UniqueID : MonoBehaviour
{
    [ReadOnlyAttr, SerializeField] private string id = "";

    // Using a static dictionary across instances might cause issues, 
    // consider alternative approaches if this is not what you intend.
    private static SerializableDictionary<string, GameObject> idDatabase = new SerializableDictionary<string, GameObject>();

    public string ID => id;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (string.IsNullOrEmpty(id) || idDatabase.ContainsKey(id))
            {
                Generate();
            }
            else
            {
                idDatabase.Add(id, this.gameObject);
            }
        }
    }

    [ContextMenu("Generate ID")]
    private void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id, this.gameObject);
    }

    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(id))
        {
            idDatabase.Remove(id);
        }
    }
}