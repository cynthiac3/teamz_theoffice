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
    private bool isUsingElevator;
    private Vector3 newPosition;
    public int currentFloor;

    //for fire alarm "Trap"
    private const string alarmTag = "FireAlarm";
    private bool atAlarm;

    //for power generator "Trap"
    private const string genTag = "PowerGenerator";
    private bool atGenerator;

    // for animation() and jump()
    private Animator anim;
    public float jumpForce;
    float xPosition;

    // For rooftop
    bool isUsingRoofTopDoor;
    private bool atRooftopDoor;
    private Vector3 roofPosition = new Vector3(6.0f,19.0f,1.0f);

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
        anim = transform.GetChild(3).GetComponent<Animator>();
        isUsingElevator = false;
        atAlarm = false;
        atGenerator = false;
        isUsingRoofTopDoor = false;
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

            //jump when Button "Jump" is pressed
            jump();

            // set animations based on speed and if grounded
            animations();

            if (atRooftopDoor) {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    mRigidbody.useGravity = false;
                    mRigidbody.detectCollisions = false;
                    showPlayer(false);
                    isUsingRoofTopDoor = true;
                }
            }

            // Moving to rooftop
            if (isUsingRoofTopDoor)
            {

                if (Mathf.Abs(mRigidbody.position.y - roofPosition.y) > 0.1)
                {
                    if (mRigidbody.position.y - roofPosition.y < 0)
                        mRigidbody.position += new Vector3(0, 0.1f, 0);
                    else
                    {
                        mRigidbody.position -= new Vector3(0, 0.1f, 0);
                    }

                }
                else
                {
                    isUsingRoofTopDoor = false;
                    mRigidbody.useGravity = true;
                    mRigidbody.detectCollisions = true;
                    showPlayer(true);
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

        //collision to allow interaction with a fire alarm
        if (other.tag.Equals(alarmTag))
        {
            atAlarm = true;
        }
        //collision to allow interaction with a power generator
        if (other.tag.Equals(genTag))
        {
            atGenerator = true;
        }
        //collision to allow interaction with rooftop door
        if (other.tag.Equals("Rooftop"))
        {
            atRooftopDoor = true;
        }

    }

    void jump()
    {
        //Vector3 pos = transform.position;
        Vector3 pos = GetComponent<Rigidbody>().velocity; // using velocity has a smoother jump but it's buggy, not sure why

        if (Input.GetButtonDown("Jump"))
        {
            //pos = new Vector3(transform.position.x, transform.position.y + jumpForce, transform.position.z);
            pos = new Vector3(pos.x, pos.y + jumpForce, pos.z);

        }

        GetComponent<Rigidbody>().velocity = pos;
        //transform.position = pos;
    }

    void animations()
    {
        if (Input.GetButtonDown("Jump"))
            anim.SetBool("IsGrounded", false);
        else
            anim.SetBool("IsGrounded", true);

        if(player==1)
            anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        else
            anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal2")));
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
        //exits fire alarm collider zone
        if (other.tag.Equals(alarmTag))
        {
            atAlarm = false;
        }
        if (other.tag.Equals(genTag))
        {
            atGenerator = false;
        }
        //collision to allow interaction with rooftop door
        if (other.tag.Equals("Rooftop"))
        {
            atRooftopDoor = false;
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
                if (isInfrontOfElevator(mRigidbody.position) && GameManager.e[currentFloor - 1].state == GameManager.Elevator.State.OPEN)
                {
                    useElevator();
                    isUsingElevator = true;
                    mRigidbody.useGravity = false;
                    mRigidbody.detectCollisions = false;
                    showPlayer(false);
                }
            }
            if(isUsingElevator)
            {

                if (Mathf.Abs(mRigidbody.position.y - newPosition.y) > 0.1)
                {
                    if (mRigidbody.position.y - newPosition.y < 0)
                        mRigidbody.position += new Vector3(0, 0.1f, 0);
                    else
                    {
                        mRigidbody.position -= new Vector3(0, 0.1f, 0);
                    }

                }
                else
                {
                    isUsingElevator = false;
                    mRigidbody.useGravity = true;
                    mRigidbody.detectCollisions = true;
                    showPlayer(true);
                }
            }
        }
        else if (player == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (isInfrontOfElevator(mRigidbody.position) && GameManager.e[currentFloor - 1].state == GameManager.Elevator.State.OPEN)
                {
                    useElevator();
                    isUsingElevator = true;
                    mRigidbody.useGravity = false;
                    mRigidbody.detectCollisions = false;
                    GetComponent<Renderer>().enabled = false;
                    showPlayer(false);

                }
            }
            if (isUsingElevator)
            {

                if (Mathf.Abs(mRigidbody.position.y - newPosition.y) > 0.1)
                {
                    if (mRigidbody.position.y - newPosition.y < 0)
                        mRigidbody.position += new Vector3(0, 0.1f, 0);
                    else
                    {
                        mRigidbody.position -= new Vector3(0, 0.1f, 0);
                    }

                }
                else
                {
                    isUsingElevator = false;
                    mRigidbody.useGravity = true;
                    mRigidbody.detectCollisions = true;
                    showPlayer(true);
                }
            }
        }
    }

    public void changeFLoorBy(int change)
    {
        GameObject floor = GameObject.Find("Floor " + (change + currentFloor));
        Vector3 pos = mRigidbody.position;
        pos.y = floor.GetComponent<Rigidbody>().transform.position.y + 0.5f;
        //mRigidbody.position = pos;
        newPosition = pos;
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
        else if(random < 0.7)
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

    //getter for use in "Sprinklers.cs"
    public bool getAlarmStatus()
    {
        return atAlarm;
    }
    //getter for use in "PowerGenerators.cs"
    public bool getGenStatus()
    {
        return atGenerator;
    }

    private void showPlayer(bool state) {
        if(transform.GetChild(1).gameObject.activeInHierarchy == true) // hide fire extinguisher if active
            transform.GetChild(1).GetComponent<Renderer>().enabled = state;
        // Hide body
        transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = state;
    }

} // end of class

