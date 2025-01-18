using UnityEngine;
// code borrowed and modified from N3K EN on youtube https://www.youtube.com/watch?v=8JHrM7yS0Xw&list=PLLH3mUGkfFCXps_IYvtPcE9vcvqmGMpRK&index=4
public class CameraBehavior : MonoBehaviour
{
    private Transform lookAt;
    private Vector3 offset;

    private void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - lookAt.position;
    }

    private void Update()
    {
        transform.position = lookAt.position + offset;
    }
}
