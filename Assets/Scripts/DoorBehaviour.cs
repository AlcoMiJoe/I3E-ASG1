using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip doorSound; // Sound to play when the door is opened or closed

    bool isOpen = false; // Indicates if the door is currently open
    public void OpenDoor()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (isOpen)
        {
            Debug.Log("Closing door"); // Log when closing the door
            doorRotation.y -= 90f; // Rotate the door back to closed position
            isOpen = false; // Set the door state to closed 
        }
        else
        {
            Debug.Log("Opening door"); // Log when opening the door
            doorRotation.y += 90f; // Rotate the door to open position
            isOpen = true; // Set the door state to open
        }
        transform.eulerAngles = doorRotation;   // Update the door's rotation
        AudioSource.PlayClipAtPoint(doorSound, transform.position); // Play the door sound at the door's position
    }
}
