using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0f; 
    
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    
    void Start()
    {
        //Initialize rigidbody physics component
        rb = GetComponent <Rigidbody>(); 
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    void FixedUpdate() 
    { 
        // create a vector movement
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        
        // add the movement vector to the object using the rigidbody component
        rb.linearVelocity = new Vector3(movementX * speed, rb.linearVelocity.y, movementY * speed); 
        
        // Only rotate if there's input
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
        }
    }
    
    void OnMove(InputValue movementValue) 
    {
        // get movement input
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        // initialize movement axis
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }
}
