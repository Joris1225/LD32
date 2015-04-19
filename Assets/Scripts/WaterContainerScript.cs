using UnityEngine;
using System.Collections;

public class WaterContainerScript : MonoBehaviour 
{
    public float maxHeight = 0.5f;
    public float fillRate = 0.05f;
    private GameObject water;
    private ParticleSystem partSystem;

    void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == "Water")
            {
                water = child;
            }
            if (child.name == "Steam")
            {
                partSystem = child.GetComponent<ParticleSystem>();
            }
        }
    }


    public void Fill()
    {
        if(water.transform.localPosition.y < maxHeight)
        {
            water.transform.Translate(new Vector3(0f, 0f, -fillRate));
        }
    }

    public void OnHeat()
    {
        if (water.transform.localPosition.y > maxHeight / 2 && !partSystem.isPlaying)
        {
            partSystem.Play();
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Mill"))
        {
            other.SendMessage("OnSteam");
        }
    }
	
}
