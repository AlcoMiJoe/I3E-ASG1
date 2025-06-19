/*
* Author: Alecxander Dela Paz
* Date: 2025-06-17
* Description: Controls the behavior of crates that can spawn collectible orbs.
*/

using UnityEngine;

/// <summary>
/// Controls the behavior of crates that can spawn collectible orbs when broken.
/// </summary>
public class CrateBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject orbPrefab; // Prefab for the orb to spawn

    [SerializeField]
    public int orbCount = 5; // Number of orbs to spawn

    public Vector3 spawnAreaPadding = new Vector3(0.1f, 0.1f, 0.1f); // Padding to avoid spawning at the very edge

    private BreakableBehaviour breakable; // Reference to the BreakableBehaviour component

    /// <summary>
    /// Called once before the first execution of Update after the MonoBehaviour is created.
    /// Subscribes to the OnBreak event of the BreakableBehaviour.
    /// </summary>
    void Start()
    {
        breakable = GetComponent<BreakableBehaviour>();
        if (breakable != null)
        {
            breakable.OnBreak += SpawnOrbs; // Subscribe to the break event
        }
        else
        {
            Debug.LogWarning("BreakableBehaviour not found on object.");
        }
    }

    /// <summary>
    /// Spawns orbs at random positions within the crate's bounds when the crate is broken.
    /// </summary>
    void SpawnOrbs()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("No collider found for spawning bounds.");
            return;
        }

        Bounds bounds = col.bounds; // Get the bounds of the collider
        Vector3 min = bounds.min + spawnAreaPadding; // Adjust min to avoid spawning at the very edge
        Vector3 max = bounds.max - spawnAreaPadding; // Adjust max to avoid spawning at the very edge

        for (int i = 0; i < orbCount; i++) // Loop to spawn the specified number of orbs
        {
            Vector3 randomPos = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y) + 0.5f, // Slightly above the crate
                Random.Range(min.z, max.z)
            );

            GameObject orb = Instantiate(orbPrefab, randomPos, Quaternion.identity);

            Rigidbody rb = orb.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1.5f),
                    Random.Range(-1f, 1f)
                ).normalized;

                float forceMagnitude = Random.Range(2f, 5f); 
                rb.AddForce(forceDir * forceMagnitude, ForceMode.Impulse);
            }
        }
    }

    void OnDestroy()
    {
        if (breakable != null)
        {
            breakable.OnBreak -= SpawnOrbs; // Unsubscribe when destroyed
        }
    }
}
