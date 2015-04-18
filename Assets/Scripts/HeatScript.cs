using UnityEngine;
using System.Collections;

public class HeatScript : MonoBehaviour 
{
    public float heatPace = 0.01f;
    public float coolPace = 0.05f;

    private float heat = 0f;
    private Material mat;

    void Awake()
    {
        Renderer r = GetComponent<Renderer>();
        mat = r.material;
    }

	void Start() 
	{
	    
	}
	
	void Update() 
	{
        mat.color = new Color(1f, 1f - heat, 1f - heat);
	}

    void FixedUpdate()
    {
        heat = Mathf.Clamp(heat - coolPace, 0f, 1f);
    }

    public void AddHeat()
    {
        heat = Mathf.Clamp(heat + heatPace, 0f, 1f);
    }
}
