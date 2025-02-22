using UnityEngine;
using UnityEngine.Events;
public class TriggerExit : MonoBehaviour
{
    public UnityEvent startEvent;

    private void Start()
    {
        startEvent.Invoke();

        /*void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
        */
    }
}
