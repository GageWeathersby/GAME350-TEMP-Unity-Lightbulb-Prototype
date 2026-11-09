using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    [Header("Movement Settings")]
    public float playerSpeed = 6.0f;
    public float gravityValue = -9.81f;
    public float jumpHeight = 1.0f;

    void Start()
    {
        // Automatically get the Character Controller component attached to the Player
        controller = GetComponent<CharacterController>();

        //lock cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the player is touching the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Gather keyboard/joystick input (WASD or Arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate directional movement vector based on the player's orientation
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Move the character horizontally
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Handle jumping physics
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Physics formula for calculating force needed to reach a specific height
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply constant downward gravity force over time
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move the character vertically (for jumping and falling)
        controller.Move(playerVelocity * Time.deltaTime);
    }
}