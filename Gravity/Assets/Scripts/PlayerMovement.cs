using System;
using UnityEngine;


struct LineCoordinates
{ 
    
}
public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRigidbody;
    Collider playerCapsuleCollider;
    

    [SerializeField]
    private float forwardSpeed = 5f;
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
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private bool changeLineAvailability = true;
    private Quaternion oldRotation = Quaternion.identity;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<Collider>();
        //  playerRigidbody.AddForce(Vector3.forward * forwardSpeed, ForceMode.VelocityChange);
    }

    void Update()
    {
        ChangeLineButtons();
        //SwitchGravityCondition();
    }
    void FixedUpdate()
    {
        playerRigidbody.AddForce(gravityDirection * gravityForce);  
        Jump();
        //jumpAvailability = true;
        //ChangeLine();
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        Vector3 halfOfCapsuleColliderExtentsByYMinusEpsilon = new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f) - new Vector3(0f, 10 * Constants.epsilon, 0f);
        if (Physics.SphereCast(transform.position - halfOfCapsuleColliderExtentsByYMinusEpsilon, playerCapsuleCollider.bounds.extents.x, targetRotation * Vector3.down, out hit, 10 * Constants.epsilon, Layer))
        {
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.AddForce(-gravityDirection * (float) Math.Sqrt(jumpHeight * (2 * gravityForce)), ForceMode.VelocityChange);
                //jumpAvailability = false;
            }
        }
    }
    void SwitchGravityCondition()
    {
        if (Input.GetKeyDown("q"))
        {
            SwitchGravity(Quaternion.Euler(0f, 0f, -90f)); 
        }

        if (Input.GetKeyDown("e"))
        {
            SwitchGravity(Quaternion.Euler(0f, 0f, 90f));       
        }
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * OldRotation, forwardSpeed * Time.deltaTime * 0.1f) * transform.rotation;
    }
    void SwitchGravity(Quaternion actualRotation)
    {
        if (changeLineAvailability == true)
        {
            targetRotation = actualRotation * targetRotation;
            gravityDirection = targetRotation * Vector3.down;
            transform.rotation = targetRotation * Quaternion.identity;
            playerRigidbody.velocity = new Vector3(0f, 0f, playerRigidbody.velocity.z);
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
                CheckBorders();
            }
            else if (Input.GetKeyDown("d"))
            {
                changeOfLineDirection = 2f;
                CheckBorders();
            }
        }
    }

    void CheckBorders()
    {
        Vector3 predictedMove = Vector3.zero;
        if (changeOfLineDirection == 1)
        {
            predictedMove.x = -Constants.predictedLengthOfMove;
        }
        else if(changeOfLineDirection == 2)
        {
            predictedMove.x = Constants.predictedLengthOfMove;
        }

        if (gravityDirection == Vector3.down)
        {
            if (Mathf.Abs((transform.position + targetRotation * predictedMove).x) < 1.5 * Constants.predictedLengthOfMove)
            {
                ChangeLine();
            }
        }

        if (gravityDirection == Vector3.right)
        {
            if (Mathf.Abs((transform.position + targetRotation * predictedMove).y) < 1.5 * Constants.predictedLengthOfMove)
            {
                ChangeLine();
            }
        }

        if (gravityDirection == Vector3.up)
        {
            if (Mathf.Abs((transform.position + targetRotation * predictedMove).x) < 1.5 * Constants.predictedLengthOfMove)
            {
                ChangeLine();
            }
        }

        if (gravityDirection == Vector3.left)
        {
            if (Mathf.Abs((transform.position + targetRotation * predictedMove).y) < 1.5 * Constants.predictedLengthOfMove)
            {
                ChangeLine();
            }
        }
    }
    void ChangeLine()
    {
        switch (changeOfLineDirection)
        {
            case 1:
                playerRigidbody.AddForce(Quaternion.Euler(0f, 0f, -90f) * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                break;
            case 2:
                playerRigidbody.AddForce(Quaternion.Euler(0f, 0f, 90f) * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                break;
            default:
                break;
        }
    }

}


