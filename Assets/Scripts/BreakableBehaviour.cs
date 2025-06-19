/*
* Author: Alecxander Dela Paz
* Date: 2025-06-17
* Description: Handles the behavior of breakable objects in the game.
*/


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


    public void Break()
    {
        Debug.Log(gameObject.name + " broken!");

        OnBreak?.Invoke(); // Invoke the break event if there are any subscribers
        AudioSource.PlayClipAtPoint(breakSound, transform.position); // Play the break sound
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
