/*
* Author: Alecxander Dela Paz
* Date: 2025-06-16
* Description: Handles the behavior of collectible orbs in the game.
*/


using UnityEngine;
using TMPro; // Importing TextMeshPro for UI text handling

// OrbBehaviour is responsible for the behavior of collectible orbs in the game
// It handles collecting the orb, playing sounds, and highlighting the orb when interacted with

public class OrbBehaviour : MonoBehaviour
{
    MeshRenderer meshRenderer;  // Reference to the MeshRenderer component for changing materials

    [SerializeField]
    AudioClip collectSound; // Sound to play when the orb is collected


    [SerializeField]
    Material HighlightMaterial; // Material to use when the orb is highlighted
    [SerializeField]
    Material DefaultMaterial; // Default material of the orb
    [SerializeField]
    int OrbValue = 1; // Value of the orb, which will be added to the player's score when collected

    [SerializeField]
    TextMeshProUGUI interactionPrompt; // Text to display when the orb is collected

    // Collects the orb, plays a sound, updates the player's score, and destroys the orb
    public void CollectOrb(PlayerBehaviour player) 
    {
        Debug.Log("Orb collected with value: " + OrbValue); // Log the collection event with the orb's value
        AudioSource.PlayClipAtPoint(collectSound, transform.position); // Play the collection sound at the orb's position
        player.ModifyScore(OrbValue); // Increase the player's score by the orb's value
        Destroy(gameObject); // Remove the orb from the scene after collection
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        DefaultMaterial = meshRenderer.material;
    }

    public void HighlightOrb()  
    {
        meshRenderer.material = HighlightMaterial; // Change the material to highlight the orb
    }
    public void UnhighlightOrb()
    {
        meshRenderer.material = DefaultMaterial; // Change the material back to default
    }
}
