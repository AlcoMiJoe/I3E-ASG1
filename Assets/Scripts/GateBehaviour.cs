using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    LeverBehaviour lever; // Reference to the lever that controls the gate
    public float MoveSpeed = 2f; // Speed at which the gate opens
    public bool isOpen = false; // Current state of the gate

    private Vector3 closedPosition; // Position when the gate is closed
    private Vector3 openPosition; // Position when the gate is open
    private Coroutine moveCoroutine; // Coroutine for moving the gate

    [SerializeField]
    AudioClip gateSound; // Sound to play when the gate is opened or closed


    void Start()
    {
        // Store the initial position as the closed position
        closedPosition = transform.position;
        // Calculate the open position based on the closed position
        openPosition = closedPosition + Vector3.up * 4f; // Adjust Y value for gate height
    }

    public void ToggleGate(bool open)
    {
        Debug.Log("Toggling gate. Open: " + open);
        isOpen = open;

        AudioSource.PlayClipAtPoint(gateSound, transform.position); // Play the gate sound

        // If a coroutine is already running, stop it before starting a new one

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // Stop any ongoing movement
        }

        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        moveCoroutine = StartCoroutine(MoveGate(targetPosition));
    }
    
    private System.Collections.IEnumerator MoveGate(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        transform.position = targetPosition; // Ensure final position is set
    }
}
