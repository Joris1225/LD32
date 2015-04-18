using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour 
{

    public float openTime = 10f;
    public float deltaY = 6f;
    public AudioClip onOpen;

    private AudioSource audioSource;
    private bool shouldMove = false;
    private Rigidbody rb;
    private float goalPosition;
    private float time;
    private float originalPosition;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position.y;
    }
	
	void Update() 
	{
	    if(shouldMove)
        {
            if (transform.position.y == goalPosition)
            {
                shouldMove = false;
                return;
            }
            rb.MovePosition(new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, goalPosition, time / openTime), transform.position.z));
            time += Time.deltaTime;
        }
	}

    public void Open()
    {
        if (!shouldMove)
        {
            time = 0f;
            audioSource.PlayOneShot(onOpen);
            shouldMove = true;
            goalPosition = originalPosition + deltaY;
        }
    }

    public void Close()
    {
        if (!shouldMove)
        {
            time = 0f;
            shouldMove = true;
            goalPosition = originalPosition;
        }
    }
}
