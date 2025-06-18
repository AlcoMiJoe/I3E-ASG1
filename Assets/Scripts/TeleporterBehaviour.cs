using UnityEngine;
using System.Collections;

public class TeleporterBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform targetPosition; // The position to teleport to
    [SerializeField]
    private AudioClip teleportSound; // Sound to play when teleporting
    [SerializeField]
    private float teleportDelay = 0.5f; // Delay before teleporting 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider is tagged as Player
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false; // Disable the character controller to prevent movement during teleportation
            }
            StartCoroutine(TeleportPlayer(other.transform)); // Start the teleportation coroutine   
            if (characterController != null)
            {
                characterController.enabled = true; // Re-enable the character controller after teleportation
            }         
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        // Play the teleport sound
        AudioSource.PlayClipAtPoint(teleportSound, transform.position);

        // Wait for the teleport delay
        yield return new WaitForSeconds(teleportDelay);

        // Teleport the player to the target position
        player.position = targetPosition.position;
        player.rotation = targetPosition.rotation; // Optionally set the player's rotation to match the target
        Debug.Log("Player teleported to: " + targetPosition.position);
    }
}
