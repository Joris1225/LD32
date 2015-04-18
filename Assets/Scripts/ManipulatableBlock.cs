using UnityEngine;
using System.Collections;

public class ManipulatableBlock : MonoBehaviour 
{
    public float heatPace = 0.01f;
    public float coolPace = 0.05f;
    public float airForce = 1f;
    public float earthForce = 10f;

    private float heat = 0f;
    private Material mat;
    private Rigidbody rb;
    private ParticleSystem steam;
    private ParticleSystem earthParticles;

    void Awake()
    {
        Renderer r = GetComponent<Renderer>();
        mat = r.material;

        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            switch (child.name)
            {
                case "Steam":
                    steam = child.GetComponent<ParticleSystem>();
                    break;
                case "Earth":
                    earthParticles = child.GetComponent<ParticleSystem>();
                    break;
            }
        }
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

    public void OnHeat()
    {
        heat = Mathf.Clamp(heat + heatPace, 0f, 1f);
    }

    public void OnAir(ParticleCollisionEvent pce)
    {
        Vector3 force = pce.velocity * airForce;
        rb.AddForce(force);
    }

    public void OnWater()
    {
        if (heat >= 0.8f && !steam.isPlaying)
        {
            steam.Play();
        }
        heat = Mathf.Clamp(heat - coolPace * 8, 0f, 1f);
    }

    public void OnEarth()
    {
        rb.AddForce(Vector3.up * earthForce, ForceMode.Impulse);
        earthParticles.Play();
    }
}
