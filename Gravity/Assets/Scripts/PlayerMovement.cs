using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;

    [SerializeField] private float gravityForce = 9.8f;

    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float changeLineSpeed = 25f;

    [SerializeField] private float changeLineTime = 0.5f;

    [SerializeField] private float changeLineLength = 10f;

    [SerializeField] private float sphereRadius = 0.5f;

    private readonly bool changeLineAvailability = true;
    private float changeOfLineDirection;
    private readonly Constants constantsInstance = new Constants();
    private Vector3 desiredLine = Vector3.zero;
    private float distanceToLine = 10f;
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private Quaternion oldRotation = Quaternion.identity;
    private Collider playerCapsuleCollider;
    private Rigidbody playerRigidbody;
    private float startingLine = 10f;


    private Quaternion targetRotation = Quaternion.identity;

    private void Awake()
    {
        desiredLine = new Vector3(0f, transform.position.y, 0f);
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<Collider>();
        //  playerRigidbody.AddForce(Vector3.forward * forwardSpeed, ForceMode.VelocityChange);
        var vector3 = new Vector3(5, 5, 5) - new Vector3(10, 5, 5);
        Debug.Log(vector3.normalized);
    }

    private void Update()
    {
        MoveToOtherLineConditions();
        SwitchGravityCondition();
        //  Debug.Log(IsEqualFloat(transform.position.x, desiredLine.x));
        // Debug.Log(transform.position.x +  " " + desiredLine.x);
        Debug.Log("update called");
    }

    private void OnGUI()
    {
        
        Debug.Log("OnGuI called");
    }

    private void FixedUpdate()
    {
        playerRigidbody.AddForce(gravityDirection * gravityForce);
        SnapToLines();
        Jump();
        //jumpAvailability = true;
        //ChangeLine();
    }

    private void Jump()
    {
        var Layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        bool onLine;
        var halfOfCapsuleColliderExtentsByYMinusEpsilon =
            new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f) -
            new Vector3(0f, 10 * Constants.Epsilon, 0f);
        jumpAvailability = Physics.SphereCast(transform.position - halfOfCapsuleColliderExtentsByYMinusEpsilon,
            playerCapsuleCollider.bounds.extents.x, gravityDirection, out hit, 10 * Constants.Epsilon, Layer);
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            if (transform.position.x == desiredLine.x)
                onLine = true;
            else
                onLine = false;
        }
        else
        {
            if (transform.position.y == desiredLine.y)
                onLine = true;
            else
                onLine = false;
        }

        if (jumpAvailability && onLine)
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                playerRigidbody.velocity = Vector3.forward * playerRigidbody.velocity.z;
                playerRigidbody.AddForce(-gravityDirection * (float) Math.Sqrt(jumpHeight * (2 * gravityForce)),
                    ForceMode.VelocityChange);
            }
    }

    private void SwitchGravityCondition()
    {
        if (Input.GetKeyDown("q")) SwitchGravity(Quaternion.Euler(0f, 0f, -90f));

        if (Input.GetKeyDown("e")) SwitchGravity(Quaternion.Euler(0f, 0f, 90f));
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation * OldRotation, forwardSpeed * Time.deltaTime * 0.1f) * transform.rotation;
    }

    private void SwitchGravity(Quaternion actualRotation)
    {
        if (changeLineAvailability)
        {
            targetRotation = actualRotation * targetRotation;
            gravityDirection = targetRotation * Vector3.down;
            transform.rotation = targetRotation * Quaternion.identity;
            playerRigidbody.velocity = new Vector3(0f, 0f, playerRigidbody.velocity.z);
            //  OldRotation = transform.rotation;
        }
    }

    private void MoveToOtherLineConditions()
    {
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            if (constantsInstance.IsEqualsFloat(transform.position.x, desiredLine.x))
            {
                if (Input.GetKeyDown("a"))
                {
                    for (var i = 0; i < constantsInstance.Lines.Length; i++)
                        if (constantsInstance.IsEqualsFloat(transform.position.x - constantsInstance.Lines[i], 6f))
                        {
                            changeOfLineDirection = -1f;
                            ChangeLine();
                            break;
                        }
                }
                else if (Input.GetKeyDown("d"))
                {
                    for (var i = 0; i < constantsInstance.Lines.Length; i++)
                        if (constantsInstance.IsEqualsFloat(transform.position.x - constantsInstance.Lines[i], -6f))
                        {
                            changeOfLineDirection = 1f;
                            ChangeLine();
                            break;
                        }
                }
            }
        }
        else
        {
            if (constantsInstance.IsEqualsFloat(transform.position.y, desiredLine.y))
            {
                if (Input.GetKeyDown("a"))
                {
                    for (var i = 0; i < constantsInstance.Lines.Length; i++)
                        if (constantsInstance.IsEqualsFloat(transform.position.y - constantsInstance.Lines[i], 6f))
                        {
                            changeOfLineDirection = -1f;
                            ChangeLine();
                            break;
                        }
                }
                else if (Input.GetKeyDown("d"))
                {
                    for (var i = 0; i < constantsInstance.Lines.Length; i++)
                        if (constantsInstance.IsEqualsFloat(transform.position.y - constantsInstance.Lines[i], -6f))
                        {
                            changeOfLineDirection = 1f;
                            ChangeLine();
                            break;
                        }
                }
            }
        }
    }

    private void ChangeLine()
    {
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            startingLine = transform.position.x;
            transform.position += new Vector3(changeOfLineDirection * Constants.Epsilon * 0.6f, 0f, 0f);
        }
        else
        {
            startingLine = transform.position.y;
            transform.position += new Vector3(0f, changeOfLineDirection * Constants.Epsilon * 0.6f, 0f);
        }
    }

    private void SnapToLines()
    {
        var numberOfLine = 0;
        var direction = Vector3.zero;
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            for (var i = 0; i < constantsInstance.Lines.Length; i++)
            {
                if (constantsInstance.Lines[i] == startingLine) continue;

                var tmpDistance = Mathf.Abs(constantsInstance.Lines[i] - transform.position.x);
                if (tmpDistance < distanceToLine)
                {
                    distanceToLine = tmpDistance;
                    numberOfLine = i;
                }
            }

            desiredLine.y = 0f;
            desiredLine.x = constantsInstance.Lines[numberOfLine];

            if (distanceToLine <= Constants.Epsilon * 10)
            {
                var tempLocation = new Vector3(desiredLine.x - transform.position.x, 0f, 0f);
                transform.position += tempLocation;
                playerRigidbody.velocity = new Vector3(0f, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            else
            {
                direction.x = desiredLine.x - transform.position.x;
                direction = direction.normalized;
                playerRigidbody.velocity = new Vector3(direction.x * changeLineSpeed, playerRigidbody.velocity.y,
                    playerRigidbody.velocity.z);
            }
        }
        else
        {
            for (var i = 0; i < constantsInstance.Lines.Length; i++)
            {
                if (constantsInstance.Lines[i] == startingLine) continue;

                var tmpDistance = Mathf.Abs(constantsInstance.Lines[i] - transform.position.y);
                if (tmpDistance < distanceToLine)
                {
                    distanceToLine = tmpDistance;
                    numberOfLine = i;
                }
            }

            desiredLine.y = constantsInstance.Lines[numberOfLine];
            desiredLine.x = 0f;

            if (distanceToLine <= Constants.Epsilon * 10)
            {
                Debug.Log(desiredLine.y);
                transform.position = new Vector3(transform.position.x, desiredLine.y, transform.position.z);
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
            }
            else
            {
                Debug.Log("2  " + desiredLine.y);
                direction.y = desiredLine.y - transform.position.y;
                direction = direction.normalized;
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, direction.y * changeLineSpeed,
                    playerRigidbody.velocity.z);
            }
        }

        distanceToLine = 10f;
    }
}