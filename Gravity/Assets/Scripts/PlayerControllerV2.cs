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
    
    private Rigidbody playerRigidbody;
    private Collider playerCapsuleCollider;

    private readonly Constants constantsInstance = new Constants();
    
    private Quaternion desiredRotation = Quaternion.identity;
    private Vector3 gravityDirection = Vector3.down;

    private float desiredLane;
    private KeyCode keyStored = KeyCode.None;
    private int state = 0; // 0 - idle, 1 - move, 2 - switch gravity
    private int previousState = 0;
    
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
        StateControl();
    }
    private void OnGUI()
    {
        var keyEvent = Event.current;
        if (!keyEvent.isKey || keyEvent.type != EventType.KeyDown || keyEvent.keyCode == KeyCode.None) 
            return;
        var keyPressed = keyEvent.keyCode;
        
        CheckInputConditions(keyPressed);
    }

    // Iteration methods
    private void StateControl()
    {
        if (state == 0)
        {
            if (previousState == 0) return;
            previousState = 0;  
            if (keyStored == KeyCode.None)
                return;
            var tmpKey = keyStored;
            keyStored = KeyCode.None;
                    
            CheckInputConditions(tmpKey);
        }
        
        if (state == 1)
            PullToLane();
        
        if(state == 2)
            RotateToGravity();
    }

    private void PullToLane()
    {
        if (constantsInstance.IsEqualVector(gravityDirection, Vector3.down) ||
            constantsInstance.IsEqualVector(gravityDirection, Vector3.up))
        {
            if (Math.Abs(desiredLane - transform.position.x) <= changeLaneSpeed * Time.fixedDeltaTime)
            {
                transform.position = new Vector3(desiredLane, transform.position.y, transform.position.z);
                state = 0;
            }
            else
            {
                var moveDirection = new Vector3(desiredLane - transform.position.x, 0, 
                    0);
                transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime; 
            }
            
        }
        else
        {
            if (Mathf.Abs(desiredLane - transform.position.y) <= changeLaneSpeed * Time.fixedDeltaTime)
            {
                transform.position = new Vector3(transform.position.x, desiredLane, transform.position.z);
                state = 0;
            }
            else
            {
                var moveDirection = new Vector3(0f, desiredLane - transform.position.y,
                    0f);
                transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime;
            }
        }

        previousState = 1;
    }

    private void RotateToGravity()
    {
        Quaternion.RotateTowards(transform.rotation, desiredRotation, rotateSpeed);
        if (transform.rotation == desiredRotation)
            state = 0;
        previousState = 2;
    }

    private void JumpControl()
    {
    }

    //Initializing methods
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
        if (keyStored != KeyCode.None) 
            return;
        
        switch (keyPressed)
        {
            case KeyCode.A:
            {
                if (state == 0)
                {
                    MoveInitialization(-1);
                    state = 1;
                }
                else
                {
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.D:
            {
                if (state == 0)
                {
                    MoveInitialization(1);
                    state = 1;
                }
                else
                {
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.Q:
            {
                if (state == 0)
                {
                    ChangeGravityDirection();
                    state = 2;
                }
                else
                {
                    keyStored = keyPressed;
                }

                break;
            }
            case KeyCode.E:
            {
                if (state == 0)
                {
                    ChangeGravityDirection();
                    state = 2;
                }
                else
                {
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