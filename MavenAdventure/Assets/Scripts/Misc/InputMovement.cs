using UnityEngine;
using UnityEngine.InputSystem;

public class InputMovement : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 movementDirection = Vector3.zero;

    public InputActionReference inputActionRef;
    void Update()
    {
        if (movementDirection != Vector3.zero)
        {
            MovePlayer();
        }
    }

    public void StartMovingUp()
    {
        movementDirection = Vector3.left;
    }

    public void StartMovingDown()
    {
        movementDirection = Vector3.right;
    }

    public void StartMovingRight()
    {
        movementDirection = Vector3.forward;
    }

    public void StartMovingLeft()
    {
        movementDirection = Vector3.back;
    }

    public void StopMoving()
    {
        movementDirection = Vector3.zero;
    }

    private void MovePlayer()
    {
        Vector3 movement = movementDirection * speed * Time.deltaTime;
        
        transform.Translate(movement);
    }

    private void OnEnable()
    {
        
    }
    
    private void OnDisable()
    {
        
    }
}