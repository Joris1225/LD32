using UnityEngine;
using System.Collections;

public class ElementsScript : MonoBehaviour 
{

    void OnParticleCollision(GameObject other)
	{
        transform.parent.SendMessage("OnParticleCollision", other);
	}


}
