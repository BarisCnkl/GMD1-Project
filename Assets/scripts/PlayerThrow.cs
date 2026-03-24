using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public Transform holdPoint;
    public float throwForce = 15f;

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
        heldStone.transform.position += new Vector3(0,-0.5f,0); // Adjust for better throw arc

        Collider col = heldStone.GetComponent<Collider>();
        col.isTrigger = false;

        heldStoneRb.isKinematic = false;
        heldStoneRb.useGravity = false;

        heldStoneRb.linearVelocity = transform.forward * throwForce;

        heldStone = null;
        heldStoneRb = null;
    }

    void OnFire(InputValue value)
    {
        Throw();
    }
}