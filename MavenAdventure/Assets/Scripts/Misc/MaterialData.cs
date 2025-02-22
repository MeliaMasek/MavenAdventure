using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class MaterialData : ScriptableObject
{
    public Material value;

    public void SetValue(Material mat)
    {
        value = mat;
    }
}