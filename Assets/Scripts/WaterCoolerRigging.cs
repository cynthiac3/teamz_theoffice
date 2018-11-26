using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCoolerRigging : MonoBehaviour {

    private bool active;
    private bool shaking;
    private bool coolDown;
    private bool atWater;
    public GameObject stunParticles;
    public GameObject waterStream;
    public GameObject usableCircle;
    private GameObject circle;
    private Player1Controller p1Script;
    private Player1Controller p2Script;

    // Use this for initialization
    void Start () {
        
        shaking = false;
        active = false;
        coolDown = false;
        atWater = false;
        circle = Instantiate(usableCircle, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90,0,0));
        
        //"imports" the player scripts so private variables can be used
        GameObject p1 = GameObject.Find("Player");
        p1Script = p1.GetComponent<Player1Controller>();
        GameObject p2 = GameObject.Find("Player2");
        p2Script = p2.GetComponent<Player1Controller>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1") && !coolDown && atWater)
        {

            //trigger turn animation
            p1Script.turnAnimation();

            coolDown = true;
            shaking = true;
            StartCoroutine(TrapIsSet());
        }
        if (Input.GetButtonDown("Fire2") && !coolDown && atWater)
        {

            //trigger turn animation
            p2Script.turnAnimation();

            coolDown = true;
            shaking = true;
            StartCoroutine(TrapIsSet());
        }
        if (shaking)
        {
            transform.position += new Vector3(Mathf.Sin(Time.time * 40.0f) * 0.005f, 0, Mathf.Sin(Time.time * 40.0f) * 0.005f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Player"))
        {
            atWater = true;

            //Trap is Triggered
            if (active)
            {
                active = false;
                Player1Controller pScript = col.GetComponent<Player1Controller>();
                GameObject stunned = Instantiate(stunParticles, col.transform);
                GameObject jet = Instantiate(waterStream, transform);
                StartCoroutine(DestroyEffects(pScript, stunned, jet));
            }
        }
        if (col.tag.Equals("Projectile"))
        {
            //Trap is Triggered
            if (active)
            {
                active = false;
                shaking = false;
                Destroy(circle);
                Destroy(col.gameObject);
                GameObject jet = Instantiate(waterStream, transform);
                StartCoroutine(KillJet(jet));
            }
        }

    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            atWater = false;
        }

    }

    public IEnumerator TrapIsSet()
    {

        active = true;
        yield return new WaitForSeconds(45);
        if(!shaking)
        {
            circle = Instantiate(usableCircle, transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90, 0, 0));
        }
        coolDown = false;
        active = false;
        shaking = false;
    }

    //CoRoutine to destroy particle effects and cause stun
    public IEnumerator DestroyEffects(Player1Controller pscript, GameObject stun, GameObject water)
    {
        yield return new WaitForSeconds(0.075f);
        Destroy(circle);
        pscript.setStun(true);
        shaking = false;
        yield return new WaitForSeconds(10);
        pscript.setStun(false);
        Destroy(stun);
        Destroy(water);
    }
    public IEnumerator KillJet(GameObject jet)
    {
        yield return new WaitForSeconds(5);
        Destroy(jet);
    }

}
