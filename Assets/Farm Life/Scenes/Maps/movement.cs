using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 3.0f;
    public float gravity = -9.81f; // Adjust gravity force here
    public float jumpHeight = 1.5f; // Adjust jump height here
    public float spawnOffset = 0.1f; // Offset from the detected ground position
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ResetPlayerPosition();
    }

    void ResetPlayerPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float initialHeight = hit.distance + spawnOffset;
            controller.center = new Vector3(0f, controller.height / 2f, 0f);
            transform.position = new Vector3(transform.position.x, initialHeight + controller.height / 2f, transform.position.z);
        }
    }

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        moveDirection.Normalize();
        moveDirection *= movementSpeed;

        // Applying gravity
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Adjust to stick to the ground better
        }

        velocity.y += gravity * Time.deltaTime;
        moveDirection.y = velocity.y;

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Collision detection
        CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
        isGrounded = (flags & CollisionFlags.Below) != 0;

        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += mouseX;

        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}