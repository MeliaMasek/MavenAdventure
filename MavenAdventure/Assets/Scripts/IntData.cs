using UnityEngine;

[CreateAssetMenu]
public class IntData : ScriptableObject
{
    public int value;
    //public UnityEvent disableEvent;
    
    public void SetValue(int num)
    {
        value = num;
    }

    public void CompareValue(IntData obj)
    {
        if (value >= obj.value)
        {
            
        }
        else
        {
            value = obj.value;
        }
    }
    
    public void SetValue(IntData obj)
    {
        value = obj.value;
    }
    
    public void UpdateValue(int number)
    {
        value += number;
    }
    
    /*private void OnDisable()
    {
        //Debug.Log("End");
        disableEvent.Invoke();
    }
    */
}