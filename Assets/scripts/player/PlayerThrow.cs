using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    
    public Transform holdPoint;
    public float throwForce = 15f;

    [SerializeField] private Camera mainCamera;
    private GameObject heldStone;
    private Rigidbody heldStoneRb;

    void Update()
    {
        // Keep stone at hold point
        if (heldStone != null)
        {
            heldStone.transform.position = holdPoint.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone") && heldStone == null)
        {
            PickUpStone(other.gameObject);
        }
    }

    void PickUpStone(GameObject stone)
    {
        heldStone = stone;
        heldStoneRb = stone.GetComponent<Rigidbody>();

        heldStoneRb.linearVelocity = Vector3.zero;
        heldStoneRb.angularVelocity = Vector3.zero;
        heldStoneRb.useGravity = false;
        heldStoneRb.isKinematic = true;

        stone.transform.SetParent(holdPoint);

        Collider col = stone.GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Throw()
    {
        if (heldStone == null) return;

        heldStone.transform.SetParent(null);

        Collider col = heldStone.GetComponent<Collider>();
        col.isTrigger = false;

        heldStoneRb.isKinematic = false;
        heldStoneRb.useGravity = true;

        StoneState stoneState = heldStone.GetComponent<StoneState>();
        if (stoneState != null)
        {
            stoneState.SetThrown();
        }

        Vector3 throwDirection = mainCamera.transform.forward;

        heldStoneRb.linearVelocity = throwDirection * throwForce;

        heldStone = null;
        heldStoneRb = null;
    }

    void OnFire(InputValue value)
    {
        Throw();
    }
}