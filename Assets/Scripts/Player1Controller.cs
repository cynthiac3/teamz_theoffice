using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Controller : MonoBehaviour
{

    public static bool gameStart = false;

    public float velocity;
    public int playerNum;

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

    //for water cooler "Trap"
    private bool stunned;

    //for throwable objects
    private bool holding;
    private int heldVers;
    public GameObject[] projectiles;
    private float facingDir;
    public int health = 100; //TEMPORARY HEALTH VARIABLE
    public int health2 = 100; //TEMPORARY HEALTH VARIABLE

    // for animation() and jump()
    private Animator anim;
    public float jumpForce;
    bool grounded;
    float xPosition;

    // For electrical hazard
    public GameObject lightningEffect;

    // For rooftop
    bool isUsingRoofTopDoor;
    private bool atRooftopDoor;
    private Vector3 roofPosition = new Vector3(6.0f,19.0f,1.0f);
    bool hasKey=false;


    // Healthbar
    public GameObject Player;
    public SimpleHealthBar healthBar;
    public Vector3 startLocation;

    //items
    public GameObject PlayerKey;
    public GameObject FireExtinguisher;
    public GameObject Item;

    //Player Floor
    public Text PlayerFloor;

    // For extinguisher
    public GameObject extinguisherPrefab;
    // For respawning
    public GameObject[] spawnpoints;


    public void PlayerHasKey(bool i)
    {
        PlayerKey.active = i;
    }

    public void PlayerHasExtinguisher(bool i)
    {
        FireExtinguisher.active = i;
    }

    public void PlayerHasItem(bool i)
    {
        Item.active = i;
    }


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
        stunned = false;
        isUsingRoofTopDoor = false;
        holding = false;
        heldVers = -1;

    }

    private void Update()
    {

        PlayerFloor.text = "Floor: " + currentFloor;
        if (gameStart && !stunned)
        {
            float inputHorizontal = Input.GetAxis("Horizontal" + playerNum);
            if (inputHorizontal != 0)
            {
                facingDir = inputHorizontal;
            }
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
                if (Input.GetButtonDown("Extinguisher" + playerNum))
                {
                    transform.GetChild(2).gameObject.SetActive(true);

                }
                if (Input.GetButtonUp("Extinguisher" + playerNum))
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                }
            }

            //jump when Button "Jump" is pressed
            if (Input.GetButtonDown("Jump" + playerNum) && isGrounded() && !isInfrontOfElevator(pos))
                jump();


            // set animations based on speed and if grounded
            animations();

            // Using the door to go to the roof top
            if (atRooftopDoor) {
                if (Input.GetButtonDown("Elevator" + playerNum))
                {
                    if (hasKey)
                    {
                        mRigidbody.useGravity = false;
                        mRigidbody.detectCollisions = false;
                        showPlayer(false);
                        isUsingRoofTopDoor = true;
                    }       
                }
            }

            // Moving to rooftop
            if (isUsingRoofTopDoor)
            {

                if (Mathf.Abs(mRigidbody.position.y - roofPosition.y) > 0.1)
                {
                    if (mRigidbody.position.y - roofPosition.y < 0)
                        mRigidbody.position += new Vector3(0, 1.0f, 0) * Time.deltaTime;
                    else
                    {
                        mRigidbody.position -= new Vector3(0, 1.0f, 0) * Time.deltaTime;
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

            //Throwing an object
            if (Input.GetButtonDown("Throw" + playerNum) && holding)
            {
                //trigger throw animation
                anim.Play("Player_Throw");

                holding = false;
                if (facingDir > 0)
                {
                    GameObject thrown = Instantiate(projectiles[heldVers], transform.position + transform.forward, Quaternion.identity);
                    Rigidbody thrownBody = thrown.GetComponent<Rigidbody>();
                    thrownBody.velocity = transform.forward * 25;
                }
                else
                {
                    GameObject thrown = Instantiate(projectiles[heldVers], transform.position - transform.forward, Quaternion.identity);
                    Rigidbody thrownBody = thrown.GetComponent<Rigidbody>();
                    thrownBody.velocity = -transform.forward * 25;
                }
            }

            if (health<=0 || health2<=0)
            {
                Die();
            }
            //  Input.GetKey(KeyCode.I)
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

        if (other.tag.Equals(fireTriggerTag))     // Touching a fire (DAMAGE)
        {
            loseHealth(40);
        }

        if (other.tag.Equals(extinguisherTriggerTag))   // Touching an extinguisher
        {
            if (!transform.GetChild(1).gameObject.active)
            {
                other.gameObject.GetComponent<PickUpObject>().destroyItem();
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    
        if (other.tag.Equals("ElecHazard"))   // Touching an electrical object (DAMAGE)
        {
            Destroy(other.gameObject.GetComponent<SphereCollider>()); // remove the collider so item stays there but doesn't affect anymore
            Instantiate(lightningEffect, transform.position + new Vector3(-0.5f,0,0), Quaternion.identity);
            anim.Play("Player_Hit");
            transform.GetComponent<AudioSource>().Play();
            loseHealth(20);
        }

        if (other.tag.Equals("PickUpItem") && !holding)   // Touching an pick up item
        {
            PickUpObject obj = other.gameObject.GetComponent<PickUpObject>();
            holding = true;
            heldVers = obj.versionIndex;
            //Debug.Log("Picked up item " + heldVers);
            obj.destroyItem();
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

    bool isGrounded()
    {
        //check if grounded
        RaycastHit hitVar = new RaycastHit();
        bool hitRay = Physics.SphereCast(transform.position - new Vector3(0, 2.05f, 0), 0.1f, -transform.up, out hitVar);//position, radius, direction, hit info
        //Debug.Log(hitVar.distance + " above surface");
        if (hitVar.distance > 0.1f)
            grounded = false;
        else
            grounded = true;

        return grounded;
    }

    void jump()
    {
        //Vector3 pos = transform.position;
        Vector3 pos = GetComponent<Rigidbody>().velocity; // using velocity has a smoother jump but it's buggy, not sure why


        //pos = new Vector3(transform.position.x, transform.position.y + jumpForce, transform.position.z);
        pos = new Vector3(pos.x, pos.y + jumpForce, pos.z);

        GetComponent<Rigidbody>().velocity = pos;
        //transform.position = pos;
    }

    void animations()
    {
        if (isGrounded())
            anim.SetBool("IsGrounded", true);
        else
            anim.SetBool("IsGrounded", false);

        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal" + playerNum)));
    }

    public void turnAnimation()
    {
        anim.Play("Player_Turn");
        transform.GetChild(3).GetComponent<PlayerModelFlip>().turnToWall();
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
        //Taking Damage from getting hit by a thrown object
        if (other.gameObject.tag.Equals("Projectile"))
        {
            //Debug.Log("I'M HIT!!!!!");
            Destroy(other.gameObject);
            anim.Play("Player_Hit");
            transform.GetComponent<AudioSource>().Play();
            loseHealth(10);
        }

        //NOTE: error being thrown if the collision isn't a floor (other collisions don't always have "parents")
        if (other.transform.parent.gameObject.name.Contains("Floor"))
        {

            int newFloor = int.Parse(other.transform.parent.gameObject.name.Split()[1]);
            if (currentFloor != newFloor)
            {
                currentFloor = newFloor;
                print(currentFloor);
            }
        }

    }

    private void elevatorDoorCheck()
    {

            if (Input.GetButtonDown("Elevator" + playerNum))
            {
                if (isInfrontOfElevator(mRigidbody.position) && GameManager.e[currentFloor - 1].state == GameManager.Elevator.State.OPEN)
                {
                    isUsingElevator = true;
                    useElevator();
                
                    if (isUsingElevator)
                    {
                        mRigidbody.useGravity = false;
                        mRigidbody.detectCollisions = false;
                        showPlayer(false);
                        mRigidbody.constraints = RigidbodyConstraints.FreezePositionX;
                    }
                }
            }
            if(isUsingElevator)
            {
                if (Mathf.Abs(mRigidbody.position.y - newPosition.y) > 0.1)
                {
                    if (mRigidbody.position.y - newPosition.y < 0)
                    { 
                        mRigidbody.position += new Vector3(0, 5.0f, 0) * Time.deltaTime;
                    }
                    else
                    {
                        mRigidbody.position -= new Vector3(0, 5.0f, 0) * Time.deltaTime;
                    }
                }
                else
                {
                    isUsingElevator = false;
                    mRigidbody.useGravity = true;
                    mRigidbody.detectCollisions = true;
                    showPlayer(true);
                    mRigidbody.constraints = RigidbodyConstraints.None;
                 }
            }
        

    }

    public void changeFLoorBy(int change)
    {
        GameObject floor = GameObject.Find("Floor " + (change + currentFloor));
        Vector3 pos = mRigidbody.position;
        print("old " + pos.y);
        pos.y = floor.transform.position.y + 0.5f;
        //mRigidbody.position = pos;
        newPosition = pos;
        print("new " + pos.y);
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
        int randomChangeFloor = getRandomChangeFloor();
        int oldFloor = currentFloor;
        int newFLoor = 0;

        if(random < 0.5)
        {
            if (currentFloor == 10)
            {
                changeFLoorBy(-randomChangeFloor);
                newFLoor = oldFloor - randomChangeFloor;
                print("down " + randomChangeFloor + " floor");
            }
            else
            {
                if(currentFloor + randomChangeFloor > 10)
                {
                    randomChangeFloor = 10 - currentFloor;
                }
                    
                changeFLoorBy(randomChangeFloor);
                newFLoor = oldFloor + randomChangeFloor;
                print("up " + randomChangeFloor + " floor");
            }
        }
        else if(random < 7.0)
        {
            if (currentFloor - randomChangeFloor < 2)
            {
                randomChangeFloor = currentFloor - 2;
            }

            if (currentFloor != 2)
            {
                changeFLoorBy(-randomChangeFloor);
            }
            else{
                isUsingElevator = false;
            }


            newFLoor = oldFloor - randomChangeFloor;
            print("down " + randomChangeFloor + " floor");
        }
        else{
            Player1Controller otherPlayer;
            int otherPlayerFloor;
            if (playerNum == 1)
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

    private int getRandomChangeFloor(){
        return ((int)(Random.value * 10 + 1)) % 4 + 1;
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
    //setter for use in "WaterCoolerRigging.cs"
    public void setStun(bool status)
    {
        stunned = status;
    }
    private void showPlayer(bool state) {
        if(transform.GetChild(1).gameObject.activeInHierarchy == true) // hide fire extinguisher if active
            transform.GetChild(1).GetComponent<Renderer>().enabled = state;
        // Hide body
        transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = state;
    }

    private void respawn()
    {
        changeFLoorBy(-currentFloor + 1);
    }

    void Die()
    {
        hasKey = false; // TODO: FIX WITH NATHAN'S CODE
        // Drop extinguisher if holding one
        if (transform.GetChild(1).gameObject.active)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            Instantiate(extinguisherPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.Euler(-90, 0, 0));
        }
        // remove pick up item
        holding = false;
        // Respawn
        if (playerNum == 1){
            transform.position = spawnpoints[0].transform.position;
            health = 100;
            healthBar.UpdateBar(health, 100);
        }
        else {
            transform.position = spawnpoints[1].transform.position;
            health2 = 100;
            healthBar.UpdateBar(health2, 100);
        }
    }

    void loseHealth(int damage) {
        if (playerNum == 1)
        {
            health -= damage;
            healthBar.UpdateBar(health, 100);
        }
        else
        {
            health2 -= damage;
            healthBar.UpdateBar(health2, 100);
        }
    }


    // Called when foam collides with fire bc the parent (the player) counts it as a collision with parent
    // and loses health
    public void fixHealth() {
        if (playerNum == 1)
        {
            health += 40;
            healthBar.UpdateBar(health, 100);
        }
        else
        {
            health2 += 40;
            healthBar.UpdateBar(health2, 100);
        }

    }

} // end of class

