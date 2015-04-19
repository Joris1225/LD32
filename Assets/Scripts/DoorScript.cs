using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour 
{

    public float moveSpeed = 0.1f;
    public float deltaY = 6f;
    public AudioClip onOpen;

    private AudioSource audioSource;
    private int direction = 0;
    private Rigidbody rb;
    private float maxY;
    private float originalY;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        originalY = transform.position.y;
        maxY = originalY + deltaY;
    }
	
	void FixedUpdate() 
	{
	    if(direction != 0)
        {
            if (direction == 1 && transform.position.y >= maxY)
                direction = 0;
            else if (direction == -1 && transform.position.y <= originalY)
                direction = 0;
            else
                rb.MovePosition(new Vector3(transform.position.x, transform.position.y + (moveSpeed * direction), transform.position.z));
        }
	}

    public void Open()
    {
        direction = 1;
    }

    public void Close()
    {
        direction = -1;   
    }
}
