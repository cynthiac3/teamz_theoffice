using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{

    public float velocity;

    private Rigidbody mRigidbody;
    private float angularVelocity;
    private bool corner;
    private float totalAngle;
    private Vector3 center;         // Center of rotation circle
    private float raduis;           // Radius of rotation circle
    private const string cornerTriggerTag = "CornerTrigger";

    // for animation() and jump()
    public Animator anim;
    public float jumpForce;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        raduis = Mathf.Abs(transform.position.z);
        angularVelocity = velocity / (2 * raduis);
        totalAngle = 0;
        corner = false;
    }

    private void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal2");
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

        //jump when Button "Jump" is pressed
        jump();

        // set animations based on speed and if grounded
        animations();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(cornerTriggerTag))     // Entering a corner
        {
            corner = true;
            center = transform.position;
            center.z = 0;
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

    void animations()
    {
        if (Input.GetButtonDown("Jump2"))
            anim.SetBool("IsGrounded", false);
        else
            anim.SetBool("IsGrounded", true);

        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal2")));

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
}