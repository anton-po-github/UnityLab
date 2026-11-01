using NUnit.Framework;

public class AngleMathTests
{
    [Test]
    public void Yaw_PlusZ_IsZero() => Assert.AreEqual(0f, AngleMath.YawDegrees(0, 0, 0, 5), 0.001f);

    [Test]
    public void Yaw_PlusX_Is90() => Assert.AreEqual(90f, AngleMath.YawDegrees(0, 0, 5, 0), 0.001f);

    [Test]
    public void Yaw_MinusZ_Is180() =>
        Assert.AreEqual(180f, System.Math.Abs(AngleMath.YawDegrees(0, 0, 0, -5)), 0.001f);

    [Test]
    public void Delta_TakesShortWayAcrossZero() =>
        Assert.AreEqual(20f, AngleMath.DeltaAngle(350f, 10f), 0.001f);

    [Test]
    public void Delta_NegativeWhenTurningLeft() =>
        Assert.AreEqual(-20f, AngleMath.DeltaAngle(10f, 350f), 0.001f);
}
