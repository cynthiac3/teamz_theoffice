using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    [SerializeField] float blocked_time = 20.0f;
    public enum State { OPEN, BLOCKED };
    public State state;
    float timer;

    // Use this for initialization
    void Start () {
        state = State.OPEN;
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == State.BLOCKED)
        {
            timer += Time.deltaTime;
            if(timer > blocked_time)
            {
                timer = 0.0f;
                state = State.OPEN;
            }
        }

	}
}
