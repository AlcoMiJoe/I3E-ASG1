/*
* Author: Alecxander Dela Paz
* Date: 2025-06-18
* Description: Handles the behavior of teleporters that move the player between locations.
*/


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

    [SerializeField]
    private bool isTeleportingToLevel2 = false; // Toggle in inspector to control audio

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            StartCoroutine(TeleportPlayer(other.transform));
            if (characterController != null)
            {
                characterController.enabled = true; // Re-enable the character controller after teleportation
            }   
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        AudioSource.PlayClipAtPoint(teleportSound, transform.position);

        yield return new WaitForSeconds(teleportDelay);

        player.position = targetPosition.position;
        player.rotation = targetPosition.rotation;
        Debug.Log("Player teleported to: " + targetPosition.position);

        // Switch background music
        if (BGMScript.Instance != null)
        {
            if (isTeleportingToLevel2)
                BGMScript.Instance.SwitchToLevel2();
            else
                BGMScript.Instance.SwitchToLevel1();
        }
    }
}
