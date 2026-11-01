using System;

// Plain C#. No Unity types, so it is easy to unit test.
public static class AngleMath
{
    // Yaw in degrees on the XZ plane. 0 = +Z (Unity forward), positive = clockwise from above.
    // Note the order: Atan2(x, z), not Atan2(z, x). This matches Unity's Y rotation.
    public static float YawDegrees(float fromX, float fromZ, float toX, float toZ)
    {
        double dx = toX - fromX;
        double dz = toZ - fromZ;
        return (float)(Math.Atan2(dx, dz) * 180.0 / Math.PI);
    }

    // Shortest signed angle from a to b, in the range [-180, 180].
    public static float DeltaAngle(float a, float b)
    {
        float d = (b - a) % 360f;
        if (d > 180f)
            d -= 360f;
        else if (d < -180f)
            d += 360f;
        return d;
    }
}
