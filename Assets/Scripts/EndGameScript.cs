using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour 
{
    public GameObject objectToSpawn;
    public int amount = 20;
    public Vector3 spawnPosition = Vector3.zero;
    public float range = 2f;
    private AudioSource audioSource;

    private bool isGameOver = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnGameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            audioSource.Play();
            for (int i = 0; i < amount; i++)
            {
                Instantiate(objectToSpawn, Random.insideUnitSphere * range + spawnPosition, Quaternion.identity);
            }
            GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
        }

    }

    
}
