using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool isOpen = false; // Indicates if the door is currently open
    public void OpenDoor()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (isOpen)
        {
            doorRotation.y -= 90f; // Rotate the door back to closed position
            isOpen = false; // Set the door state to closed 
        }
        else
        {
            doorRotation.y += 90f; // Rotate the door to open position
            isOpen = true; // Set the door state to open
        }
        transform.eulerAngles = doorRotation;   // Update the door's rotation
    }
}
