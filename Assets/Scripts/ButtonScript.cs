using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{
    public string triggerEnterMessage = string.Empty;
    public string triggerExitMessage = string.Empty;
    public GameObject receiver;

    private int numObjectsInTrigger = 0;

    void OnTriggerEnter(Collider other)
    {
        if (numObjectsInTrigger == 0 && triggerEnterMessage != string.Empty)
            receiver.SendMessage(triggerEnterMessage);
        numObjectsInTrigger++;
    }

    void OnTriggerExit(Collider other)
    {
        numObjectsInTrigger--;
        if (numObjectsInTrigger == 0 && triggerExitMessage != string.Empty)
            receiver.SendMessage(triggerExitMessage);
    }
}
