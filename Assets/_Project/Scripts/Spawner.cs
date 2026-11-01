using UnityEngine;

// Spawns a prefab at a fixed interval, at a random point in a square area.
public class Spawner : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject prefab;

    [Header("Settings")]
    [Range(0.1f, 5f)]
    [SerializeField]
    private float intervalSeconds = 1f;

    [Range(0f, 10f)]
    [SerializeField]
    private float areaSize = 4f;

    [Tooltip("Seconds before a spawned object is destroyed.")]
    [Range(1f, 30f)]
    [SerializeField]
    private float lifetimeSeconds = 8f;

    private float timer;

    // Runs in the Editor when a value changes in the Inspector.
    private void OnValidate()
    {
        intervalSeconds = Mathf.Max(0.1f, intervalSeconds);
    }

    private void Update()
    {
        // Timer instead of a coroutine: no allocation, easy to read.
        timer += Time.deltaTime;
        if (timer < intervalSeconds)
            return;

        timer -= intervalSeconds;
        Spawn();
    }

    private void Spawn()
    {
        if (prefab == null)
            return;

        Vector2 offset = Random.insideUnitCircle * areaSize;
        Vector3 position = transform.position + new Vector3(offset.x, 0f, offset.y);

        GameObject instance = Instantiate(prefab, position, Random.rotation);
        Destroy(instance, lifetimeSeconds);
    }

    // Draws the spawn area in the Scene view when the object is selected.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaSize);
    }
}
