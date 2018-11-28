using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donuts : MonoBehaviour {


    public GameObject donuts;
    public float spawnTime;
    private Player1Controller p1Script;
    private Player1Controller p2Script;
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("donutSpawn", spawnTime, spawnTime);
        //"imports" the player scripts so private variables can be used
        GameObject p1 = GameObject.Find("Player");
        p1Script = p1.GetComponent<Player1Controller>();
        GameObject p2 = GameObject.Find("Player2");
        p2Script = p2.GetComponent<Player1Controller>();
    }
    
    //CoRoutines for each player to stay slowed for 10 seconds
    
    public IEnumerator BoostP1()
    {
        yield return new WaitForSeconds(10); //amount of time for the player to be slowed
        p1Script.velocity /= 4;
        yield return new WaitForSeconds(10);
    }
    public IEnumerator BoostP2()
    {
        yield return new WaitForSeconds(10); //amount of time for the player to be slowed
        p2Script.velocity /= 4;
        yield return new WaitForSeconds(10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("Player 1 boosted");
            StartCoroutine(BoostP1());
        }
        if (other.tag.Equals("Player2"))
        {
            Debug.Log("Player 2 boosted");
            StartCoroutine(BoostP2());
        }

    }

    void donutSpawn()
    {
        int donutSpawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(donuts, spawnPoints[donutSpawnIndex].position, spawnPoints[donutSpawnIndex].rotation);
    }
}
