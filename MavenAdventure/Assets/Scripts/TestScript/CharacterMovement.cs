using UnityEngine;
using UnityEngine.InputSystem;

//code borrowed and modified from 
public class CharacterMovement : MonoBehaviour

{
    [SerializeField] private Animator animator;
    public InputActionReference playerInput;
    private CharacterController controller;
    private Transform camMain;
    [SerializeField] private float rotationSpeed = 4f;

    [SerializeField] private FloatData speed;
    private float minSpeed = 1.0f;
    
    public Vector3 resetPos;
    public Quaternion resetPosRot;
    
    private void Awake()
    {
        //playerInput = new LookChar(); //Which makes this is not needed
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        camMain = Camera.main.transform;
    }

    private void OnEnable()
    {
        playerInput.action.Enable();
    }

    private void OnDisable()
    {
        playerInput.action.Disable();
    }
    void Update()
    {
        Vector2 movementInput = playerInput.action.ReadValue<Vector2>();
        Vector3 move = (camMain.forward * movementInput.y + camMain.right * movementInput.x);
        move.y = 0f;
        controller.Move(move * (Time.deltaTime * speed.value));
        if  (movementInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + camMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            animator.SetBool("IsWalking", true);

        }
        else
        {
            animator.SetBool("IsWalking", false);

        }
    }
    
    public void ResetPlayerLocation()
    {
        Vector3 moveVector = resetPos - transform.position;

        controller.Move(moveVector);
        
        transform.rotation = resetPosRot;

        Debug.Log("Resetting player position and rotation.");
    }
}