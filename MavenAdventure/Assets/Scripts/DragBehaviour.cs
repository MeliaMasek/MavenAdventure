using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DragBehaviour : MonoBehaviour
{
    private Camera cameraObj;
    public bool drag;
    public Vector3 position, offset;
    public UnityEvent startDragEvent, endDragEvent;
    void Start()
    {
        cameraObj = Camera.main;
    }
    
    public IEnumerator OnMouseDown()
    {
        offset = transform.position - cameraObj.ScreenToWorldPoint(Input.mousePosition);
        drag = true;
        startDragEvent.Invoke();
        yield return new WaitForFixedUpdate();
        
        while (drag)
        {
            yield return new WaitForFixedUpdate();
            position = cameraObj.ScreenToWorldPoint(Input.mousePosition) + offset;
            //Debug.Log(offset);
            transform.position = position;
        }
    }

    private void OnMouseUp()
    {
        drag = false;
        endDragEvent.Invoke();
    }
}