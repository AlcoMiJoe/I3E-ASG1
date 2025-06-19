/*
* Author: Alecxander Dela Paz
* Date: 2025-06-18
* Description: Handles the behavior of hazards that can instant kill the player.
*/

using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{

    /// <summary>
    /// Handles the trigger event when the player enters the hazard area.
    /// This method checks if the collider that entered the trigger is tagged as "Player".
    /// If so, it retrieves the PlayerBehaviour component and calls the ModifyHealth method
    /// to apply damage, effectively simulating an instant kill.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider is tagged as Player
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.ModifyHealth(-999); // Call the TakeDamage method on the player
                Debug.Log("Player entered hazard area and took damage.");
            }
        }
    }
}