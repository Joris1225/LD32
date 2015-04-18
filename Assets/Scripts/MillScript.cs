using UnityEngine;
using System.Collections;

public class MillScript : MonoBehaviour 
{
    public GameObject door;
    public float turnSpeed = 0.5f;
    private bool shouldRotate = false;

    void FixedUpdate()
    {
        if(shouldRotate)
        {
            transform.Rotate(new Vector3(0, 0, turnSpeed));
        }
    }

    public void OnSteam()
    {
        shouldRotate = true;
        door.SendMessage("Open");
    }
	
}
