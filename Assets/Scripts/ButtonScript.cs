using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{
    public string triggerEnterMessage = string.Empty;
    public string triggerExitMessage = string.Empty;
    public GameObject receiver;

    void OnTriggerEnter(Collider other)
    {
        if (triggerEnterMessage != string.Empty)
            receiver.SendMessage(triggerEnterMessage);
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerExitMessage != string.Empty)
            receiver.SendMessage(triggerExitMessage);
    }
}
