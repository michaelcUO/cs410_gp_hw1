using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player; // Reference to the player GameObject.
    private Vector3 offset; // The distance between the camera and the player.
    // Start is called before the first frame update.
    void Start() {
        // Calculate the initial offset between the camera's position and the player's position.
        offset = transform.position - player.transform.position; 
    }

    // LateUpdate is called once per frame after all Update functions have been completed.
    void LateUpdate() {
        // Maintain the same offset between the camera and player.
        transform.position = player.transform.position + offset; // Update the camera position based on the player's position and the offset.
    }
}
