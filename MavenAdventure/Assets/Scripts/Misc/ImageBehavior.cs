using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageBehavior : MonoBehaviour
{
    public GameAction gameActionObj;
    public UnityEvent startEvent;
    private Image img;

    private void Start()
    {
        gameActionObj.raise += RunStartEvent;
        img = GetComponent<Image>();
        startEvent.Invoke();
    }

    public void RunStartEvent()
    {
        startEvent.Invoke();
    }
    
    public void UpdateImage(FloatData obj)
    {
        img.fillAmount = obj.value;
    }
}
