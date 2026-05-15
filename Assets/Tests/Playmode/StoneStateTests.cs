using NUnit.Framework;
using UnityEngine;

public class StoneStateTests
{
    [Test]
    public void StoneStartsAsGround()
    {
        GameObject stoneObject = new GameObject();
        StoneState stoneState = stoneObject.AddComponent<StoneState>();

        Assert.IsTrue(stoneState.IsGround);
        Assert.IsFalse(stoneState.IsHeld);
        Assert.IsFalse(stoneState.IsThrown);

        Object.DestroyImmediate(stoneObject);
    }

    [Test]
    public void StoneCanBeSetToHeld()
    {
        GameObject stoneObject = new GameObject();
        StoneState stoneState = stoneObject.AddComponent<StoneState>();

        stoneState.SetHeld();

        Assert.IsTrue(stoneState.IsHeld);
        Assert.IsFalse(stoneState.IsGround);
        Assert.IsFalse(stoneState.IsThrown);

        Object.DestroyImmediate(stoneObject);
    }

    [Test]
    public void StoneCanBeSetToThrown()
    {
        GameObject stoneObject = new GameObject();
        StoneState stoneState = stoneObject.AddComponent<StoneState>();

        stoneState.SetThrown();

        Assert.IsTrue(stoneState.IsThrown);
        Assert.IsFalse(stoneState.IsGround);
        Assert.IsFalse(stoneState.IsHeld);

        Object.DestroyImmediate(stoneObject);
    }
}