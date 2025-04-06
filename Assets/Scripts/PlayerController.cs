using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    // Rigidbody of the player.
    private Rigidbody rb;

    // Variable to keep track of collected "PickUp" objects.
    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    // UI text component to display count of "PickUp" objects collected.
    public TextMeshProUGUI countText;

    // UI object to display winning text.
    public GameObject winTextObject;

    // For jump functionality.
    public float jumpForce = 5f; // Force applied when jumping.
    public int maxJumps = 2; // Maximum number of jumps allowed.
    private int jumpCount; // Current number of jumps performed.
    private bool isGrounded; // Flag to check if the player is on the ground.
    private bool jumpPressed = false; // Flag to check if jump is pressed.

    // Start is called before the first frame update.
    void Start() {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Initialize count to zero.
        count = 0;

        // Update the count display.
        SetCountText();

        // Initially set the win text to be inactive.
        winTextObject.SetActive(false);

        jumpCount = 0; // Initialize jump count.
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue) {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Jump uses 'Update' to reliably check for jump input since it is called every frame
    void Update() {
        // Check if the space key is pressed.
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpPressed = true; // Set jumpPressed to true when space is pressed.
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate() {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);

        // If a jump is pressed and the player hasn't exceeded the max jumps:
        if (jumpPressed && jumpCount < maxJumps) {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity to ensure consistent jump height.
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply upward force for jumping.
        jumpCount++; // Increment the jump count.
        }
        jumpPressed = false; // Reset the flag
    }

    // Trigger event handler for when the player enters a trigger collider.
    void OnTriggerEnter(Collider other) {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            // Increment the count of "PickUp" objects collected.
            count++;

            // Update the count display.
            SetCountText();
        }
    }

    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText() {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        // Check if the count has reached or exceeded the win condition.
        if (count >= 12)
        {
            // Display the win text.
            winTextObject.SetActive(true);

            // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    // Collision event handler for when the player collides with another object.
    private void OnCollisionEnter(Collision collision) {

        // Check if the player touches the ground.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set the grounded flag to true.
            // Reset jump count when the player is on the ground.
            jumpCount = 0;
        }


        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object.
            Destroy(gameObject);

            // Update the winText to display "You Lose!"
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
    
}