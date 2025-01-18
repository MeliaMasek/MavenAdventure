using System.Globalization;
using UnityEngine;

[CreateAssetMenu]

//All code is for testing purposes and is sourced from Anthony Romrell
public class FloatData : ScriptableObject
{
    public float value;

    public void SetValue(float num)
    {
        value = num;
    }
    public void UpdateValue(float num)

    {
        value += num;
    }
}