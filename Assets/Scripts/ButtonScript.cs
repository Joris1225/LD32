using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{
    public string message;
    public GameObject receiver;

    void OnTriggerEnter(Collider other)
    {
        receiver.SendMessage(message);
    }
}
