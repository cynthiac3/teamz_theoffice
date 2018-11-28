using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinklers : MonoBehaviour {

    public GameObject sprinklersOrg;
    public GameObject particleShower;
    private Player1Controller p1Script;
    private Player1Controller p2Script;
    private GameObject[] sprinklerArr;
    private bool alarmCoolDown;

    // Use this for initialization
    void Start()
    {
        //first set of sprinklers are built into the scene(on 2nd floor)
        //the rest are instantiated using a y displacement for each floor
        float ydisplacement = 2.8f;
        sprinklerArr = new GameObject[8];
        for (int i = 0; i < 8; i++)
        {
            sprinklerArr[i] = Instantiate(sprinklersOrg, new Vector3(1, ydisplacement, 1), Quaternion.identity);
            ydisplacement += 1.8f;

        }
        //"imports" the player scripts so private variables can be used
        GameObject p1 = GameObject.Find("Player");
        p1Script = p1.GetComponent<Player1Controller>();
        GameObject p2 = GameObject.Find("Player2");
        p2Script = p2.GetComponent<Player1Controller>();
        alarmCoolDown = false;
    }

    void Update()
    {
        //PLAYER 1 triggers the fire alarm
        if (Input.GetButtonDown("Fire1") && !alarmCoolDown && p1Script.getAlarmStatus())
        {
            alarmCoolDown = true;
            int p2Lvl = p2Script.getCurrentFloor();
            int p1Lvl = p1Script.getCurrentFloor();

            //trigger turn animation
            p1Script.turnAnimation();

            //In the event that both players are on the same floor when switch is pulled
            if (p1Lvl == p2Lvl)
            {
                p1Script.velocity = p1Script.velocity / 4;
                StartCoroutine(SoakP1());
                p2Script.velocity = p2Script.velocity / 4;
                StartCoroutine(SoakP2());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklerArr[p2Lvl - 3].transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }
            }
            //if statement for Prefab sprinkler sets
            else if (p2Lvl > 2 && p2Lvl < 10)
            {
                p2Script.velocity = p2Script.velocity / 4;
                StartCoroutine(SoakP2());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklerArr[p2Lvl - 3].transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }

            }
            //if statement for sprinkler set that is apart of the scene
            else if (p2Lvl == 2)
            {
                p2Script.velocity = p2Script.velocity / 4;
                StartCoroutine(SoakP2());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklersOrg.transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }
            }
            else
            {
                alarmCoolDown = false;
            }
        }

        //PLAYER 2 triggers the fire alarm
        if (Input.GetButtonDown("Fire2") && !alarmCoolDown && p2Script.getAlarmStatus())
        {
            alarmCoolDown = true;
            int p2Lvl = p2Script.getCurrentFloor();
            int p1Lvl = p1Script.getCurrentFloor();

            //trigger turn animation
            p2Script.turnAnimation();

            //In the event that both players are on the same floor when switch is pulled
            if (p1Lvl == p2Lvl)
            {
                p1Script.velocity = p1Script.velocity / 4;
                StartCoroutine(SoakP1());
                p2Script.velocity = p2Script.velocity / 4;
                StartCoroutine(SoakP2());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklerArr[p2Lvl - 3].transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }
            }
            //if statement for Prefab sprinkler sets
            else if (p1Lvl > 2 && p1Lvl < 10)
            {
                p1Script.velocity = p1Script.velocity / 4;
                StartCoroutine(SoakP1());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklerArr[p1Lvl - 3].transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }

            }
            //if statement for sprinkler set that is apart of the scene
            else if (p1Lvl == 2)
            {
                p1Script.velocity = p1Script.velocity / 4;
                StartCoroutine(SoakP1());
                for (int i = 0; i < 12; i++)
                {
                    Transform sprinklerShower = sprinklersOrg.transform.GetChild(i);
                    GameObject shower = Instantiate(particleShower, sprinklerShower.position, Quaternion.Euler(-90, 0, 0));
                    StartCoroutine(ShowerDestroyer(shower));
                }
            }
            else
            {
                alarmCoolDown = false;
            }
        }
    }

    //CoRoutines for each player to stay slowed for 10 seconds
    public IEnumerator SoakP2()
    {
        yield return new WaitForSeconds(10); //amount of time for the player to be slowed
        p2Script.velocity *= 4;
        yield return new WaitForSeconds(10);
        alarmCoolDown = false;
    }
    public IEnumerator SoakP1()
    {
        yield return new WaitForSeconds(10); //amount of time for the player to be slowed
        p1Script.velocity *= 4;
        yield return new WaitForSeconds(10);
        alarmCoolDown = false;
    }

    //CoRoutine for destroying the particle object spawned at each sprinkler
    public IEnumerator ShowerDestroyer(GameObject shower)
    {
        yield return new WaitForSeconds(15);
        Destroy(shower);
        
    }

    //getter for use in "FireAlarms.cs"
    public bool getCD()
    {
        return alarmCoolDown;
    }
}