using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private GameObject player;
    private Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found. Please ensure the player has the tag 'Player'.");
        }
        else
        {
            offset = transform.position - player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}