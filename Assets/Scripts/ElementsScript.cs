using UnityEngine;
using System.Collections;

public class ElementsScript : MonoBehaviour 
{

    private ParticleSystem partSystem;

	void Awake() 
	{
        partSystem = GetComponentInChildren<ParticleSystem>();
	}
	
	void Update() 
	{
        if (Input.GetButton("Fire1"))
        {
            if (!partSystem.isPlaying)
                partSystem.Play();
        }
        else
        {
            if (partSystem.isPlaying)
                partSystem.Stop();
        }
	}

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("HeatBlock"))
        {
            other.SendMessage("AddHeat");
        }
    }
}
