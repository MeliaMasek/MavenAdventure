using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class TextData : ScriptableObject
{
    public Text value;

    public void SetValue(Text text)
    {
        value = text;
    }
}