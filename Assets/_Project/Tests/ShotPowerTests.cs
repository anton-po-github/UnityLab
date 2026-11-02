using NUnit.Framework;

public class ShotPowerTests
{
    [Test]
    public void ZeroPower_GivesMinImpulse() =>
        Assert.AreEqual(0.5f, ShotPower.ToImpulse(0f, 0.5f, 10f), 0.0001f);

    [Test]
    public void FullPower_GivesMaxImpulse() =>
        Assert.AreEqual(10f, ShotPower.ToImpulse(1f, 0.5f, 10f), 0.0001f);

    [Test]
    public void PowerAboveOne_IsClamped() =>
        Assert.AreEqual(10f, ShotPower.ToImpulse(5f, 0.5f, 10f), 0.0001f);

    [Test]
    public void HalfDrag_GivesHalfPower() =>
        Assert.AreEqual(0.5f, ShotPower.DragToPower01(1.5f, 3f), 0.0001f);

    [Test]
    public void ZeroMaxDrag_GivesZeroPower() =>
        Assert.AreEqual(0f, ShotPower.DragToPower01(1f, 0f), 0.0001f);
}
