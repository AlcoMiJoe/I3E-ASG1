using UnityEngine;

public class AxeBehaviour : MonoBehaviour
{
    public void CollectAxe(PlayerBehaviour player)
    {
        player.HasAxe = true;
        Debug.Log("Axe collected!");
        Destroy(gameObject); // Destroy the axe object after collection
    }
}
