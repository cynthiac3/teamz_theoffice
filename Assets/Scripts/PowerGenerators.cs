using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerators : MonoBehaviour {

    public GameObject[] generatorSet;
    public GameObject blindParticles;
    public GameObject cam1;
    public GameObject cam2;
    private Player1Controller p1Script;
    private Player1Controller p2Script;
    private bool genCoolDown;

    // Use this for initialization
    void Start()
    {
        float yDisplacement = 0;
        int genVariant = Random.Range(0, 2);

        for (int i = 0; i < 5; i++)
        {
            
            GameObject newGenerator = Instantiate(generatorSet[genVariant], transform);
            newGenerator.transform.position += new Vector3(0, yDisplacement, 0);
            yDisplacement += 1.8f;
            if (genVariant == 0)
            {
                genVariant++;
            }
            else
            {
                genVariant--;
            }
        }

        GameObject p1 = GameObject.Find("Player");
        p1Script = p1.GetComponent<Player1Controller>();
        GameObject p2 = GameObject.Find("Player2");
        p2Script = p2.GetComponent<Player1Controller>();
        genCoolDown = false;

    }
    // Update is called once per frame
    void Update () {
        //PLAYER 1 triggers the fire alarm
        if (Input.GetButtonDown("Fire1") && !genCoolDown && p1Script.getGenStatus())
        {
            genCoolDown = true;
            int p2Lvl = p2Script.getCurrentFloor();
            int p1Lvl = p1Script.getCurrentFloor();
            //In the event that both players are on the same floor when switch is pulled
            if (p1Lvl == p2Lvl)
            {
                GameObject blind1 = Instantiate(blindParticles, cam1.transform);
                blind1.layer = 9; //LAYER NUMBER should be equal to "P1View"
                GameObject blind2 = Instantiate(blindParticles, cam2.transform);
                blind2.layer = 10; //LAYER NUMBER should be equal to "P2View"
                StartCoroutine(BlindDestroyer(blind1));
                StartCoroutine(BlindDestroyer(blind2));
            }
            
            else
            {
                GameObject blind2 = Instantiate(blindParticles, cam2.transform);
                blind2.layer = 10; //LAYER NUMBER should be equal to "P2View"
                StartCoroutine(BlindDestroyer(blind2));
            }
        }

        //PLAYER 2 triggers the fire alarm
        if (Input.GetButtonDown("Fire2") && !genCoolDown && p2Script.getGenStatus())
        {
            genCoolDown = true;
            int p2Lvl = p2Script.getCurrentFloor();
            int p1Lvl = p1Script.getCurrentFloor();
            //In the event that both players are on the same floor when switch is pulled
            if (p1Lvl == p2Lvl)
            {
                GameObject blind1 = Instantiate(blindParticles, cam1.transform);
                blind1.layer = 9; //LAYER NUMBER should be equal to "P1View"
                GameObject blind2 = Instantiate(blindParticles, cam2.transform);
                blind2.layer = 10; //LAYER NUMBER should be equal to "P2View"
                StartCoroutine(BlindDestroyer(blind1));
                StartCoroutine(BlindDestroyer(blind2));
            }
            
            else
            {
                GameObject blind1 = Instantiate(blindParticles, cam1.transform);
                blind1.layer = 9; //LAYER NUMBER should be equal to "P1View"
                StartCoroutine(BlindDestroyer(blind1));
            }
        }
    }

    //CoRoutine to desroy particle effect and end cooldown after 15 seconds
    public IEnumerator BlindDestroyer(GameObject blind)
    {
        yield return new WaitForSeconds(15);
        Destroy(blind);
        genCoolDown = false;
    }
}
