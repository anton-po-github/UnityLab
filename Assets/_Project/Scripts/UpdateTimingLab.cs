using UnityEngine;

// Counts Update, FixedUpdate and LateUpdate calls and logs the result once per second.
public class UpdateTimingLab : MonoBehaviour
{
    [Range(10, 120)]
    [SerializeField]
    private int targetFrameRate = 30;

    private int updateCount;
    private int fixedCount;
    private int lateCount;
    private float timer;

    private void Awake()
    {
        // vSync overrides targetFrameRate, so turn it off for this test.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    private void Update() => updateCount++;

    private void FixedUpdate() => fixedCount++;

    private void LateUpdate()
    {
        lateCount++;

        timer += Time.unscaledDeltaTime;
        if (timer < 1f)
            return;

        Debug.Log(
            $"Last {timer:F2}s: Update={updateCount}, FixedUpdate={fixedCount}, LateUpdate={lateCount}"
        );
        updateCount = 0;
        fixedCount = 0;
        lateCount = 0;
        timer = 0f;
    }

    // Applies the new value when you change it in the Inspector during Play.
    private void OnValidate()
    {
        if (Application.isPlaying)
            Application.targetFrameRate = targetFrameRate;
    }
}
