using UnityEngine;

public class HazardBehaviour : MonoBehaviour
{
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