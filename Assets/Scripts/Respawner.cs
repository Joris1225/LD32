using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour 
{

	void OnTriggerEnter(Collider col)
    {
        RespawnScript rs = col.GetComponent<RespawnScript>();
        if(rs != null)
        {
            rs.Respawn();
        }
    }
}
