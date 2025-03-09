using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public InputActionReference playerInput;
    private CharacterController controller;
    private Transform camMain;

    [SerializeField] private FloatData speed;

    public Transform characterMovementObject;  // This should be your CharacterController object (for movement)
    public Transform characterMeshObject;     // This should be your mesh (for rotation)

    private void Awake()
    {
        controller = characterMovementObject.GetComponent<CharacterController>();
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

        // Get the forward and right vectors of the camera
        Vector3 camForward = camMain.forward;
        Vector3 camRight = camMain.right;

        // Normalize the vectors to prevent diagonal movement from being faster
        camForward.y = 0f; // Flatten the forward vector to keep movement grounded
        camRight.y = 0f;   // Flatten the right vector to keep movement grounded
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction based on camera orientation
        Vector3 moveDirection = (camForward * movementInput.y + camRight * movementInput.x);

        if (movementInput.sqrMagnitude > 0.01f) // Move only if there's significant input
        {
            // Move the character by using CharacterController on the movement object
            characterMovementObject.Translate(moveDirection * (Time.deltaTime * speed.value));

            // Rotate the character mesh to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            characterMeshObject.rotation = Quaternion.Slerp(characterMeshObject.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}
