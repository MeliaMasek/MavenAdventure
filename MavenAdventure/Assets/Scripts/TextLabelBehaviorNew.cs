using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextLabelBehaviorNew : MonoBehaviour
{
    public Text label;
    public IntData score;
    
    //public FloatData dataObj;
    public UnityEvent startEvent;
    private void Start()
    {
        label = GetComponent<Text>();
        startEvent.Invoke();
        //label.text = dataObj.value.ToString();
    }

    private void Update()
    {
        // Check if the Text component and ScoreData have been assigned.
        if (label != null && score != null)
        {
            // Update the UI Text with the current score from the ScoreData.
            label.text = score.value.ToString();
        }
    }

    public void UpdateLabel(FloatData obj)
    {
        label.text = obj.value.ToString(CultureInfo.InvariantCulture);
        //label.text = dataObj.value.ToString(CultureInfo.InvariantCulture);
    }

    public void UpdateLabel(IntData obj)
    {
        label.text = obj.value.ToString(CultureInfo.InvariantCulture);
    }
}