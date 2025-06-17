using UnityEngine;

public class CrateBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject orbPrefab;
    [SerializeField]
    public int orbCount = 5;
    public Vector3 spawnAreaPadding = new Vector3(0.1f, 0.1f, 0.1f);

    private BreakableBehaviour breakable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        breakable = GetComponent<BreakableBehaviour>();
        if (breakable != null)
        {
            breakable.OnBreak += SpawnOrbs;
        }
        else
        {
            Debug.LogWarning("BreakableBehaviour not found on object.");
        }
    }
    
    void SpawnOrbs()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("No collider found for spawning bounds.");
            return;
        }

        Bounds bounds = col.bounds;
        Vector3 min = bounds.min + spawnAreaPadding;
        Vector3 max = bounds.max - spawnAreaPadding;

        for (int i = 0; i < orbCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
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
