/**
* Author: Alecxander Dela Paz
* Date: 2025-06-14
* Description: Handles door interactions, including opening and closing doors.
*/


using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip doorSound; // Sound to play when the door is opened or closed

    bool isOpen = false; // Indicates if the door is currently open

    /// <summary>
    /// Toggles the door state between open and closed.
    /// When the door is opened, it rotates 90 degrees around the Y-axis.
    /// When closed, it rotates back to its original position.
    /// Plays a sound effect when the door is toggled.
    /// </summary>
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
