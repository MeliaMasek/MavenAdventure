using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CoroutineBehavoir : MonoBehaviour
{
    public bool canRun;
    public UnityEvent startEvent, startCountEvent, repeatCountEvent, endCountEvent, repeatUntilFalseEvent;
    public IntData counterNum;
    public float seconds = 3f;
    private WaitForSeconds wfsObj;
    private WaitForFixedUpdate wffuObj;

    public bool CanRun
    {
        get => canRun;
        set => canRun = value;
    }


    private void Start()
    {
        startEvent.Invoke();
        wfsObj = new WaitForSeconds(seconds);
        wffuObj = new WaitForFixedUpdate();
    }


    public void StartCounting()
    {
        StartCoroutine(Counting());
    }
    
    
    private IEnumerator Counting()
    {
    startCountEvent.Invoke();
    yield return wfsObj;
        while (counterNum.value > 0)
        {
            repeatCountEvent.Invoke();
            counterNum.value--;
            yield return wfsObj;
        }
        endCountEvent.Invoke();
    }

    public void StartRepeatUntilFalse()
    {
        CanRun = true;
        StartCoroutine(RepeatUntilFalse());
    }
    
    private IEnumerator RepeatUntilFalse()
    {
        while (CanRun)
        {
            yield return wfsObj;
            repeatUntilFalseEvent.Invoke();
        }
    }
}
