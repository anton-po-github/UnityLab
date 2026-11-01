using UnityEngine;

// Logs the lifecycle order. Per-frame methods log only the first 3 frames,
// so the Console does not flood.
public class LifecycleLogger : MonoBehaviour
{
    private const int MaxFramesToLog = 3;
    private int updateCount;
    private int fixedCount;
    private int lateCount;

    private void Awake()
    {
        Log("Awake");
    }

    private void OnEnable()
    {
        Log("OnEnable");
    }

    private void Start()
    {
        Log("Start");
    }

    private void FixedUpdate()
    {
        if (fixedCount++ < MaxFramesToLog)
            Log("FixedUpdate");
    }

    private void Update()
    {
        if (updateCount++ < MaxFramesToLog)
            Log("Update");
    }

    private void LateUpdate()
    {
        if (lateCount++ < MaxFramesToLog)
            Log("LateUpdate");
    }

    private void OnDisable() => Log("OnDisable");

    private void OnDestroy() => Log("OnDestroy");

    private void Log(string method)
    {
        Debug.Log($"[{Time.frameCount}] {name}: {method}", this);
    }
}
