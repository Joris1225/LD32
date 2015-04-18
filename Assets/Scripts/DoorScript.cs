using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour 
{

    public float openTime = 10f;

    private bool shouldOpen = false;
    private Rigidbody rb;
    private float goalPosition;
    private float time;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	void Start() 
	{
	
	}
	
	void Update() 
	{
	    if(shouldOpen)
        {
            if (transform.position.y >= goalPosition)
            {
                this.enabled = false;
                return;
            }
            rb.MovePosition(new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, goalPosition, time / openTime), transform.position.z));
            time += Time.deltaTime;
        }
	}

    public void Open()
    {
        if(!shouldOpen)
        {
            shouldOpen = true;
            goalPosition = transform.position.y + 6f;
        }
        
    }
}
