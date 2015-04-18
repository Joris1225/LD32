using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour 
{
    public float liftSpeed = 1f;
    private Rigidbody rb;

	void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	public void Ascend()
    {
        rb.AddForce(Vector3.up * liftSpeed);
    }
}
