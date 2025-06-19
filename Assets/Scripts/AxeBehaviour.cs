/*
* Author: Alecxander Dela Paz
* Date: 2025-06-15
* Description: Handles axe interactions, including collection by the player.
*/


using UnityEngine;

public class AxeBehaviour : MonoBehaviour
{
    [SerializeField]
    AudioClip collectSound; // Sound to play when the axe is collected

    public void CollectAxe(PlayerBehaviour player)
    {
        player.HasAxe = true;
        Debug.Log("Axe collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position); // Play the collection sound
        Destroy(gameObject); // Destroy the axe object after collection
    }
}
