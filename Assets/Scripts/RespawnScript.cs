using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour 
{
    private Vector3 spawnPosition;
    private Rigidbody rb;

	void Start() 
	{
        rb = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
	}

    public void Respawn()
    {
        transform.position = spawnPosition;
        if(rb != null)
            rb.velocity = Vector3.zero;
    }
}
