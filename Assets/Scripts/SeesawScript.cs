using UnityEngine;
using System.Collections;

public class SeesawScript : MonoBehaviour 
{
    private WaterContainerScript waterContainer;
    private bool full = false;

    void Awake()
    {
        waterContainer = GetComponentInChildren<WaterContainerScript>();
    }

	void Update() 
	{
        if (!full && waterContainer.WaterLevel > 1.12f)
        {
            full = true;
            GameObject.Find("EndGameManager").GetComponent<EndGameScript>().OnGameOver();
        }
        float z = waterContainer.WaterLevel * 5 - 2.5f;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
	}

    public void Fill()
    {
        waterContainer.Fill();
    }
}
