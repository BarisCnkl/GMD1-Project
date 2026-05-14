using UnityEngine;

public class StoneState : MonoBehaviour
{
    public enum State
    {
        Ground,
        Held,
        Thrown
    }

    [SerializeField] private State currentState = State.Ground;

    public State CurrentState => currentState;

    public bool IsGround => currentState == State.Ground;
    public bool IsHeld => currentState == State.Held;
    public bool IsThrown => currentState == State.Thrown;

    public void SetGround()
    {
        currentState = State.Ground;
    }

    public void SetHeld()
    {
        currentState = State.Held;
    }

    public void SetThrown()
    {
        currentState = State.Thrown;
    }
}