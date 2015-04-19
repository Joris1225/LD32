using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour 
{

    public float moveSpeed = 0.1f;
    public float deltaY = 6f;
    public AudioClip onOpen;

    private AudioSource audioSource;
    private int direction = 0;
    private float maxY;
    private float originalY;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
                transform.Translate(new Vector3(0f, moveSpeed * direction, 0f));
        }
	}

    public void Open()
    {
        direction = 1;
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(onOpen);
    }

    public void Close()
    {
        direction = -1;   
    }
}
