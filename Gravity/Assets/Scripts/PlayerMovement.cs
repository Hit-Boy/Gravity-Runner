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

    private float playerHeight;
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private Quaternion targetRotation = Quaternion.identity;
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
        SwitchGravityCondition();
    }
    void FixedUpdate()
    {
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Direction = targetRotation * Direction;
        PlayerRigidbody.MovePosition(transform.position +  Direction * Time.deltaTime * speed);
        PlayerRigidbody.AddForce(gravityDirection * gravityForce);
        Jump();
        jumpAvailability = true;
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
        targetRotation = actualRotation * targetRotation;
        gravityDirection = targetRotation * Vector3.down;
        transform.rotation = targetRotation * Quaternion.identity;
        PlayerRigidbody.velocity = new Vector3(0f, 0f, PlayerRigidbody.velocity.z);
        //  OldRotation = transform.rotation;
    }
}


