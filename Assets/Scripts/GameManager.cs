using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameObject[] elevators;
    public static Elevator[] e;
    [SerializeField] float blocked_time;

	// Use this for initialization
	void Start () {
        elevators = new GameObject[10];
        elevators = GameObject.FindGameObjectsWithTag("elevator");
        e = new Elevator[elevators.Length];


        for (int i = 0; i < elevators.Length; ++i)
        {
            elevators[i] = GameObject.Find("Floor " + (i + 1)).transform.GetChild(6).GetChild(1).gameObject;
            elevators[i].gameObject.GetComponent<Light>().color = Color.green;
            e[i] = new Elevator();
        }
        elevators[0].GetComponent<Light>().color = Color.red;
        e[0].state = Elevator.State.CLOSED;

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < e.Length; ++i)
        {
            if(e[i].state == Elevator.State.CLOSED)
            {
                e[i].timer += Time.deltaTime;
                if(e[i].timer > blocked_time)
                {
                    e[i].timer = 0.0f;
                    e[i].state = Elevator.State.OPEN;
                    elevators[i].gameObject.GetComponent<Light>().color = Color.green;
                }
            }
        }
    }
    public class Elevator{
        public enum State { OPEN, CLOSED };
        public State state;
        public float timer;
        public Elevator(){
            state = State.OPEN;
        }
    }

}
