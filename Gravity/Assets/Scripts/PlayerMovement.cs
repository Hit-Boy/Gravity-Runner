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
    private float changeLineSpeed = 0.5f;
    [SerializeField]
    private float changeLineTime = 0.2f;

    private Quaternion targetRotation = Quaternion.identity;
    private float changeOfLineDirection = 0f;
    private float playerHeight;
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private bool changeLineAvailability = true;
    private Quaternion oldRotation = Quaternion.identity;
    private Quaternion rotateRight = Quaternion.Euler(0, 0, 90);
    private Quaternion rotateLeft = Quaternion.Euler(0, 0, -90);

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        playerHeight = GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        ChangeLineButtons();
        SwitchGravityCondition();
    }
    void FixedUpdate()
    {
        Vector3 Direction = new Vector3(0, 0f, 1);
        PlayerRigidbody.MovePosition(transform.position +  Direction * Time.deltaTime * speed);
        PlayerRigidbody.AddForce(gravityDirection * gravityForce);
        Jump();
        jumpAvailability = true;
        ChangeLine();
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        if (jumpAvailability && Physics.Raycast(transform.position, gravityDirection, playerHeight/2 + Constants.epsilon, Layer))
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
            SwitchGravity(rotateLeft); 
        }

        if (Input.GetKeyDown("e"))
        {
            SwitchGravity(rotateRight);       
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
                    PlayerRigidbody.AddForce(rotateLeft * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                    changeLineAvailability = false;
                    break;
                case 2:
                    PlayerRigidbody.AddForce(rotateRight * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
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
                    PlayerRigidbody.AddForce(rotateRight * gravityDirection * changeLineSpeed / changeLineTime);
                    break;
                case 2:
                    PlayerRigidbody.AddForce(rotateLeft * gravityDirection * changeLineSpeed / changeLineTime);
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


