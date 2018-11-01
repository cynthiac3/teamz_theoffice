using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour {

    public GameObject keys;
    public float spawnTime;
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start() {
        InvokeRepeating("keySpawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update() {

    }

    void keySpawn() {
        int keySpawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(keys, spawnPoints[keySpawnIndex].position, spawnPoints[keySpawnIndex].rotation);
    }
}
