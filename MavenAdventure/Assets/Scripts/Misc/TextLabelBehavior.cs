using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextLabelBehavior : MonoBehaviour
{
    private Text label;
    //public FloatData dataObj;
    public UnityEvent startEvent;
    private void Start()
    {
        label = GetComponent<Text>();
        startEvent.Invoke();
        //label.text = dataObj.value.ToString();
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
