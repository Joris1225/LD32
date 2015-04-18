using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{

    public GameObject door;

    void OnTriggerEnter(Collider other)
    {
        door.SendMessage("Open");
    }
}
