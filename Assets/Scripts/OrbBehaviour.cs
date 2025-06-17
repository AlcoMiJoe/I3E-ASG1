using UnityEngine;
using TMPro; // Importing TextMeshPro for UI text handling

// OrbBehaviour is responsible for the behavior of collectible orbs in the game
// It handles collecting the orb, playing sounds, and highlighting the orb when interacted with

public class OrbBehaviour : MonoBehaviour
{
    MeshRenderer meshRenderer;

    [SerializeField]
    AudioClip collectSound; // Sound to play when the orb is collected


    [SerializeField]
    Material HighlightMaterial;
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    int OrbValue = 1;

    [SerializeField]
    TextMeshProUGUI interactionPrompt; // Text to display when the orb is collected

    public void CollectOrb(PlayerBehaviour player)
    {
        Debug.Log("Orb collected with value: " + OrbValue);
        AudioSource.PlayClipAtPoint(collectSound, transform.position); // Play the collection sound
        player.ModifyScore(OrbValue); // Modify the player's score by the orb's value
        Destroy(gameObject); // Destroy the orb after collection
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
