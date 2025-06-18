using UnityEngine;
using System.Collections;

// LeverBehaviour controls a lever that can be flipped by the player
// The lever requires a certain score to be flipped and controls a gate's state
// When flipped, it changes its rotation to indicate its state and notifies the gate to open or close

public class LeverBehaviour : MonoBehaviour
{
    public int targetScore = 1000; // Score needed to activate the lever
    public bool isOn = false; // Current lever state

    [SerializeField]
    AudioClip leverSound; // Sound to play when the lever is flipped

    GateBehaviour gate; // Reference to the gate this lever controls

    Quaternion offRotation;
    Quaternion onRotation;

    void Start()
    {
        // Store both on and off rotations
        offRotation = transform.rotation;
        onRotation = Quaternion.Euler(transform.eulerAngles.x - 120f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    // This method attempts to flip the lever if the player meets the score requirement
    public bool TryFlipLever(PlayerBehaviour player)
    {
        if (player.currentScore < targetScore)
        {
            return false; // Not enough score to flip lever
        }

        isOn = !isOn;
        transform.rotation = isOn ? onRotation : offRotation;
        gate.ToggleGate(isOn); // Notify the gate to open or close
        AudioSource.PlayClipAtPoint(leverSound, transform.position); // Play the lever sound
        Debug.Log("Lever flipped. New state: " + (isOn ? "ON" : "OFF"));
        return true;
    }
}
