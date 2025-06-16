using UnityEngine;

public class OrbBehaviour : MonoBehaviour
{
    MeshRenderer meshRenderer;

    [SerializeField]
    Material HighlightMaterial;
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    int OrbValue = 1;

    public void CollectOrb(PlayerBehaviour player)
    {
        Debug.Log("Orb collected with value: " + OrbValue);
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
