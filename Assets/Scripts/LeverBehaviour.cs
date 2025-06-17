using UnityEngine;

public class LeverBehaviour : MonoBehaviour
{
    PlayerBehaviour player;
    public int targetScore = 1000;
    public bool IsActive = true;
    public bool IsFlipped = false;

    // void Update()
    // {
    //     if (player != null && player.currentScore >= targetScore)
    //     {
    //         IsActive = true;
    //     }
    // }

    public void FlipLever()
    {
        if (IsActive)
        {
            if (IsFlipped == false)
            {
                IsFlipped = true;
                transform.rotation = Quaternion.Euler(-120f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // Flip the lever
                Debug.Log("Lever flipped!");
                // Add any additional logic for when the lever is flipped, such as triggering an event or changing game state
            }
            else
            {
                IsFlipped = false;
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // Reset the lever
                Debug.Log("Lever reset!");
                // Add any additional logic for when the lever is reset
            }
        }
        else
        {
            Debug.LogWarning("Lever is not active. Cannot flip.");
        }
    }
}
