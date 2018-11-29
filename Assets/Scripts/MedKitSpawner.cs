using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitSpawner : MonoBehaviour {

    public GameObject medkits;
    public float spawnTime;
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("medkitSpawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void medkitSpawn()
    {
        int medKitSpawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(medkits, spawnPoints[medKitSpawnIndex].position, spawnPoints[medKitSpawnIndex].rotation);
    }
}