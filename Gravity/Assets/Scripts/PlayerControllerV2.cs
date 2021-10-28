using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerControllerV2 : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;

    [SerializeField] private float gravityForce = 9.8f;

    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float changeLaneSpeed = 25f;

    [SerializeField] private float rotateSpeed = 25f;

    private readonly Constants constantsInstance = new Constants();

    private float desiredLane;
    private readonly Quaternion desiredRotation = Quaternion.identity;
    private readonly Vector3 gravityDirection = Vector3.down;
    private bool inAir = false;

    private Rigidbody playerRigidbody;
    private Collider playerCapsuleCollider;
    private bool pullNeeded = false;
    private bool rotateNeeded = false;
    private KeyCode keyStored = KeyCode.None;
    private int state = 0; // 0 - idle, 1 - move, 2 - switch gravity
    


    public PlayerControllerV2(Collider playerCapsuleCollider, Rigidbody playerRigidbody)
    {
        this.playerCapsuleCollider = playerCapsuleCollider;
        this.playerRigidbody = playerRigidbody;
    }

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }
    private void OnGUI()
    {
        Debug.Log("OnGuI called");
        var keyEvent = Event.current;
        if (!keyEvent.isKey || keyEvent.type != EventType.KeyDown || keyEvent.keyCode == KeyCode.None) return;
        var keyPressed = keyEvent.keyCode;

        if (keyPressed == KeyCode.A)
        {
            if (true)
                return;
        }

    }

    // Iteration methods
    private void PullToLane()
    {
        if (constantsInstance.IsEqualVector(gravityDirection, Vector3.down) ||
            constantsInstance.IsEqualVector(gravityDirection, Vector3.up))
        {
            if (constantsInstance.IsEqualsFloat(desiredLane, transform.position.x))
                return;
            
            var moveDirection = new Vector3(desiredLane - transform.position.x, transform.position.y,
                transform.position.z);
            transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime;
        }
        else
        {
            if (constantsInstance.IsEqualsFloat(desiredLane, transform.position.y))
                return;
            
            var moveDirection = new Vector3(transform.position.x, desiredLane - transform.position.y,
                transform.position.z);
            transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime;
        }
    }

    private void RotateToGravity()
    {
        Quaternion.RotateTowards(transform.rotation, desiredRotation, rotateSpeed);
    }

    private void JumpControl()
    {
    }

    //Initializing methods
    private void StateCheck()
    {
        if (true)
            return;
    }

    private void Jump()
    {
        playerRigidbody.velocity = Vector3.forward * playerRigidbody.velocity.z;
        playerRigidbody.AddForce(-gravityDirection * (float) Math.Sqrt(jumpHeight * (2 * gravityForce)),
            ForceMode.VelocityChange); 
    }
    private void MoveInitialization(int direction)
    {
            
    }

    private void ChangeGravityDirection()
    {
        
    }
    
    //Input methods
    private void CheckInputConditions(KeyCode keyPressed)
    {
        switch (keyPressed)
        {
            case KeyCode.A:
            {
                if (state == 0)
                    MoveInitialization(-1);
                else
                {
                    if (keyStored != KeyCode.None) return;
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.D:
            {
                if (state == 0)
                    MoveInitialization(1);
                else
                {
                    if (keyStored != KeyCode.None) return;
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.Q:
            {
                if (state == 0)
                    ChangeGravityDirection();
                else
                {
                    if (keyStored != KeyCode.None) return;
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.E:
            {
                if (state == 0)
                    ChangeGravityDirection();
                else
                {
                    if (keyStored != KeyCode.None) return;
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.Space:
            {
                if (state == 0 && OnGroundCheck())
                    Jump();
                else
                {
                    if (keyStored != KeyCode.None) return;
                    keyStored = keyPressed;
                }

                break;
            }
            default:
                break;
        }
        
    }
    
    //Additional methods
    private void FindDesiredLane()
    {
        if (constantsInstance.IsEqualVector(gravityDirection, Vector3.down) ||
            constantsInstance.IsEqualVector(gravityDirection, Vector3.up))
        {
            var distanceToLane = 10f;
            var numberOfDesiredLane = 3;
            for (var i = 0; i < constantsInstance.Lines.Length; i++)
            {
                var tmpDistance = Mathf.Abs(constantsInstance.Lines[i] - transform.position.x);
                if (!(tmpDistance < distanceToLane)) continue;
                distanceToLane = tmpDistance;
                numberOfDesiredLane = i;
            }

            desiredLane = constantsInstance.Lines[numberOfDesiredLane];
        }
        else
        {
            var distanceToLane = 10f;
            var numberOfDesiredLane = 3;
            for (var i = 0; i < constantsInstance.Lines.Length; i++)
            {
                var tmpDistance = Mathf.Abs(constantsInstance.Lines[i] - transform.position.y);
                if (!(tmpDistance < distanceToLane)) continue;
                distanceToLane = tmpDistance;
                numberOfDesiredLane = i;
            }

            desiredLane = constantsInstance.Lines[numberOfDesiredLane];
        }
    }
    private bool OnGroundCheck()
    {
        var layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        bool onLine;
        var halfOfCapsuleColliderExtentsByYMinusEpsilon =
            new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f) -
            new Vector3(0f, 10 * Constants.Epsilon, 0f);
        return Physics.SphereCast(transform.position - halfOfCapsuleColliderExtentsByYMinusEpsilon,
            playerCapsuleCollider.bounds.extents.x, gravityDirection, out hit, 10 * Constants.Epsilon, layer);
    }
}