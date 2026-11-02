using UnityEngine;

// Pure logic: drag distance -> power (0..1) -> impulse. No scene objects, easy to test.
public static class ShotPower
{
    public static float DragToPower01(float dragDistance, float maxDrag)
    {
        if (maxDrag <= 0f)
            return 0f;
        return Mathf.Clamp01(dragDistance / maxDrag);
    }

    // Squared curve: small pulls give fine control, big pulls give a hard shot.
    public static float ToImpulse(float power01, float minImpulse, float maxImpulse)
    {
        float p = Mathf.Clamp01(power01);
        return Mathf.Lerp(minImpulse, maxImpulse, p * p);
    }
}
