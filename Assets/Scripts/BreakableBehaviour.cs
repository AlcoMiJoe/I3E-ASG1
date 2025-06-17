using UnityEngine;
using System;
using TMPro; // Importing TextMeshPro for UI text handling

public class BreakableBehaviour : MonoBehaviour
{
    MeshRenderer meshRenderer; // Reference to the MeshRenderer component

    [SerializeField]
    Material highlightMaterial; // Material to use for highlighting the object when interacted with
    [SerializeField]    
    Material originalMaterial; // Original material to restore after highlighting

    [SerializeField]
    AudioClip breakSound; // Sound to play when the object breaks
    public event Action OnBreak; // Event to notify when the object breaks

    [SerializeField]
    TextMeshProUGUI interactionPrompt; // Text to display when the object breaks
    public void Break()
    {
        Debug.Log(gameObject.name + " broken!");

        OnBreak?.Invoke(); // Invoke the break event if there are any subscribers
        AudioSource.PlayClipAtPoint(breakSound, transform.position); // Play the break sound
        interactionPrompt.text = ""; // Clear the interaction prompt text
        interactionPrompt.gameObject.SetActive(false); // Hide the interaction prompt
        Destroy(gameObject);
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
    }

    public void Highlight()
    {
        meshRenderer.material = highlightMaterial;
    }

    public void Unhighlight()
    {
        meshRenderer.material = originalMaterial; // Restore the original material
    }
}
