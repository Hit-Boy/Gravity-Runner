using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody PlayerRigidbody;
    

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float gravityForce = 9.8f;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float changeLineSpeed = 5f;
    [SerializeField]
    private float changeLineTime = 0.5f;
    [SerializeField]
    private float changeLineLength = 10f;


    private Quaternion targetRotation = Quaternion.identity;
    private float changeOfLineDirection = 0f;
    private float playerHeight;
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private bool changeLineAvailability = true;
    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion rotateGravityRight = Quaternion.Euler(0, 0, 90);
    private Quaternion rotateGravityLeft = Quaternion.Euler(0, 0, -90);

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        playerHeight = GetComponent<Collider>().bounds.size.y;
        PlayerRigidbody.velocity = transform.forward * Time.deltaTime * speed;
    }

    void Update()
    {
        ChangeLineButtons();
        SwitchGravityCondition();
    }
    void FixedUpdate()
    {
        Vector3 Direction = new Vector3(1f, 0f, 0f);
        PlayerRigidbody.MovePosition(transform.position +  Direction * Time.deltaTime * speed);
        Debug.Log(PlayerRigidbody.velocity);
        PlayerRigidbody.AddForce(gravityDirection * gravityForce);
        Jump();
        jumpAvailability = true;
        ChangeLine();
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        if (jumpAvailability && Physics.Raycast(transform.position, gravityDirection, playerHeight/2 + Constants.epsilon, Layer) && changeLineAvailability)
        {
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                PlayerRigidbody.velocity = Vector3.zero;
                PlayerRigidbody.AddForce(-gravityDirection * (float) Math.Sqrt(jumpHeight * (2 * gravityForce)), ForceMode.VelocityChange);
                jumpAvailability = false;
            }
        }
    }
    void SwitchGravityCondition()
    {
        if (Input.GetKeyDown("q"))
        {
            SwitchGravity(rotateGravityLeft); 
        }

        if (Input.GetKeyDown("e"))
        {
            SwitchGravity(rotateGravityRight);       
        }
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * OldRotation, speed * Time.deltaTime * 0.1f) * transform.rotation;
    }
    void SwitchGravity(Quaternion actualRotation)
    {
        if (changeLineAvailability == true)
        {
            targetRotation = actualRotation * targetRotation;
            gravityDirection = targetRotation * Vector3.down;
            transform.rotation = targetRotation * Quaternion.identity;
            PlayerRigidbody.velocity = new Vector3(0f, 0f, PlayerRigidbody.velocity.z);
            //  OldRotation = transform.rotation;
        }
    }

    void ChangeLineButtons()
    {
        if (changeLineAvailability == true)
        {
            if (Input.GetKeyDown("a"))
            {
                changeOfLineDirection = 1f;
            }
            else if (Input.GetKeyDown("d"))
            {
                changeOfLineDirection = 2f;
            }
            else
                changeOfLineDirection = 0f;
        }
    }
    void ChangeLine()
    {
        if (changeLineAvailability == true & changeOfLineDirection !=0f)
        {
            switch (changeOfLineDirection)
            {
                case 1:
                    PlayerRigidbody.AddForce(rotateGravityLeft * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                    changeLineAvailability = false;
                    break;
                case 2:
                    PlayerRigidbody.AddForce(rotateGravityRight * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                    changeLineAvailability = false;
                    break;
                default:
                    break;
            }
        }

        

        if (changeLineAvailability == false)
        {
            switch (changeOfLineDirection)
            {
                case 1:
                    PlayerRigidbody.AddForce(rotateGravityRight * gravityDirection * Mathf.Sqrt((2 * changeLineLength / changeLineTime) - (2 * changeLineSpeed)));
                    break;
                case 2:
                    PlayerRigidbody.AddForce(rotateGravityLeft * gravityDirection * Mathf.Sqrt((2 * changeLineLength / changeLineTime) - (2 * changeLineSpeed)));
                    break;
                default:
                    break;
            }
        }
    }

    void CheckBorderLine()
    { 
        
    }

}


