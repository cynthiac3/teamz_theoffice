using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{

    public static bool gameStart = false;

    public float velocity;
    public int player;


    private Rigidbody mRigidbody;
    private float angularVelocity;
    private bool corner;
    private float totalAngle;
    private Vector3 center;         // Center of rotation circle
    private float raduis;           // Radius of rotation circle
    private const string cornerTriggerTag = "CornerTrigger";
    private const string fireTriggerTag = "Fire";
    private const string extinguisherTriggerTag = "Extinguisher";
    private int currentFloor;



    public static void StartGame()
    {
        gameStart = true;
    }


    private void Start()
    {

        mRigidbody = GetComponent<Rigidbody>();
        raduis = Mathf.Abs(transform.position.z);
        angularVelocity = velocity / (2 * raduis);
        totalAngle = 0;
        corner = false;
        currentFloor = 1;
    }

    private void Update()
    {


        if (gameStart)
        {
            float inputHorizontal = Input.GetAxis((player == 1) ? "Horizontal" : "Horizontal2");
            elevatorDoorCheck();
            
            Vector3 pos = mRigidbody.position;

            if (!corner)        // Move the character in a straight line
            {
                Vector3 movement = transform.forward * velocity * inputHorizontal * Time.deltaTime;
                pos += movement;

                // Round depth to +radius or -raduis            
                int sign = (int)Mathf.Round(pos.z / Mathf.Abs(pos.z));
                pos.z = sign * raduis;
            }
            else            // Move the character in a circular motion
            {
                totalAngle += -inputHorizontal * angularVelocity * Time.deltaTime;
                pos.x = center.x + raduis * Mathf.Sin(totalAngle);
                pos.z = center.z + raduis * Mathf.Cos(totalAngle);
            }

            mRigidbody.position = pos;
            mRigidbody.rotation = Quaternion.AngleAxis(totalAngle * Mathf.Rad2Deg - 90, Vector3.up);

            // Activate fire extinguisher spray
            if (transform.GetChild(1).gameObject.activeInHierarchy)
            {
                if (Input.GetKey(KeyCode.T))
                {
                    transform.GetChild(2).gameObject.SetActive(true);

                }
                else if (!Input.GetKey(KeyCode.T))
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                }
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(cornerTriggerTag))     // Entering a corner
        {
            corner = true;
            center = transform.position;
            center.z = 0;
        }

        if (other.tag.Equals(fireTriggerTag))     // Touching a fire
        {
            // how much the character should be knocked back
            var magnitude = 100;
            // calculate force vector
            var force = transform.position - other.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            mRigidbody.AddForce(force * magnitude);
        }

        if (other.tag.Equals(extinguisherTriggerTag))   // Touching an extinguisher
        {
            Destroy(other.gameObject);
            transform.GetChild(1).gameObject.SetActive(true);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(cornerTriggerTag))
        {                                           // Exiting a corner

            corner = false;

            // Round rotation to k * PI
            // Find k
            float k = Mathf.Round(totalAngle / (Mathf.PI));
            totalAngle = k * Mathf.PI;

        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.name.Contains("Floor"))
        {

            int newFloor = int.Parse(other.gameObject.name.Split()[1]);
            if (currentFloor != newFloor)
            {
                currentFloor = newFloor;
                print(currentFloor);
            }
        }
    }

    private void elevatorDoorCheck()
    {
        if (player == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (isInfrontOfElevator(mRigidbody.position) && GameManager.e[currentFloor-1].state == GameManager.Elevator.State.OPEN)
                    useElevator();
            }
        }
        else if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (isInfrontOfElevator(mRigidbody.position) && GameManager.e[currentFloor-1].state == GameManager.Elevator.State.OPEN)
                    useElevator();
            }
        }
    }

    public void changeFLoorBy(int change)
    {
        GameObject floor = GameObject.Find("Floor " + (change + currentFloor));
        Vector3 pos = mRigidbody.position;
        pos.y = floor.GetComponent<Rigidbody>().transform.position.y + 0.5f;
        mRigidbody.position = pos;
    }

    private bool isInfrontOfElevator(Vector3 pos)
    {
        return (pos.x > -1 && pos.x < 1 && pos.z < 0);
    }

    public int getCurrentFloor()
    {
        return currentFloor;
    }

    private void useElevator(){
        GameManager.elevators[currentFloor-1].gameObject.GetComponent<Light>().color = Color.red;
        GameManager.e[currentFloor-1].state = GameManager.Elevator.State.CLOSED;
        float random = Random.value;
        int oldFloor = currentFloor;
        int newFLoor = 0;
        if(random < 0.5)
        {
            changeFLoorBy(1);
            newFLoor = oldFloor + 1;
            print("up 1 floor");
        }
        else if(random < 0.3)
        {
            if (currentFloor != 1)
            {
                changeFLoorBy(-1);
                newFLoor = oldFloor - 1;
            }
            print("down 1 floor");
        }
        else{
            Player1Controller otherPlayer;
            int otherPlayerFloor;
            if (player == 1)
            {
                otherPlayer = GameObject.Find("Player2").GetComponent<Player1Controller>();
                otherPlayerFloor = otherPlayer.currentFloor;
                newFLoor = otherPlayerFloor;
            }
            else{
                otherPlayer = GameObject.Find("Player").GetComponent<Player1Controller>();
                otherPlayerFloor = otherPlayer.currentFloor;
                newFLoor = otherPlayerFloor;
            }
            changeFLoorBy(otherPlayerFloor - currentFloor);
            print("oponent floor");
        }

        GameManager.elevators[newFLoor-1].gameObject.GetComponent<Light>().color = Color.red;
        GameManager.e[newFLoor-1].state = GameManager.Elevator.State.CLOSED;
    }


}
