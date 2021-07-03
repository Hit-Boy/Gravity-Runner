using System;
using UnityEngine;

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
    private float changeLineSpeed = 25f;
    [SerializeField]
    private float changeLineTime = 0.5f;
    [SerializeField]
    private float changeLineLength = 10f;
    [SerializeField]
    private float sphereRadius = 0.5f;


    private Quaternion targetRotation = Quaternion.identity;
    private float changeOfLineDirection = 0f;
    private Vector3 gravityDirection = Vector3.down;
    private bool jumpAvailability = true;
    private bool changeLineAvailability = true;
    private Quaternion oldRotation = Quaternion.identity;
    private Vector3 desiredLine = Vector3.zero;
    private float startingLine = 10f;
    Constants constantsInstance = new Constants();
    private float distanceToLine = 10f;

    private void Awake()
    {
        desiredLine = new Vector3(0f, transform.position.y, 0f);
    }
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<Collider>();
        //  playerRigidbody.AddForce(Vector3.forward * forwardSpeed, ForceMode.VelocityChange);

    }

    void Update()
    {
        ChangeLineConditions();
        SwitchGravityCondition();
        //  Debug.Log(IsEqualFloat(transform.position.x, desiredLine.x));
        // Debug.Log(transform.position.x +  " " + desiredLine.x);
    }
    void FixedUpdate()
    {
        playerRigidbody.AddForce(gravityDirection * gravityForce);
        SnapToLines();
        Jump();
        //jumpAvailability = true;
        //ChangeLine();
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        bool onLine;
        Vector3 halfOfCapsuleColliderExtentsByYMinusEpsilon = new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f) - new Vector3(0f, 10 * Constants.epsilon, 0f);
        jumpAvailability = Physics.SphereCast(transform.position - halfOfCapsuleColliderExtentsByYMinusEpsilon, playerCapsuleCollider.bounds.extents.x, gravityDirection, out hit, 10 * Constants.epsilon, Layer);
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            if (transform.position.x == desiredLine.x)
            {
                onLine = true;
            }
            else
            {
                onLine = false;
            }
        }
        else
        {
            if (transform.position.y == desiredLine.y)
            {
                onLine = true;
            }
            else
            {
                onLine = false;
            }
        }
        if (jumpAvailability && onLine)
        {
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                playerRigidbody.velocity = Vector3.forward * playerRigidbody.velocity.z;
                playerRigidbody.AddForce(-gravityDirection * (float)Math.Sqrt(jumpHeight * (2 * gravityForce)), ForceMode.VelocityChange);
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

    private void ChangeLineConditions()
    {
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            if (constantsInstance.IsEqualsFloat(transform.position.x, desiredLine.x))
            {
                if (Input.GetKeyDown("a"))
                {
                    for (int i = 0; i < constantsInstance.lines.Length; i++)
                    {
                        if (constantsInstance.IsEqualsFloat(transform.position.x - constantsInstance.lines[i], 6f))
                        {
                            changeOfLineDirection = -1f;
                            ChangeLine();
                            break;
                        }
                    }
                }
                else if (Input.GetKeyDown("d"))
                {
                    for (int i = 0; i < constantsInstance.lines.Length; i++)
                    {
                        if (constantsInstance.IsEqualsFloat(transform.position.x - constantsInstance.lines[i], -6f))
                        {
                            changeOfLineDirection = 1f;
                            ChangeLine();
                            break;
                        }
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
                    for (int i = 0; i < constantsInstance.lines.Length; i++)
                    {
                        if (constantsInstance.IsEqualsFloat(transform.position.y - constantsInstance.lines[i], 6f))
                        {
                            changeOfLineDirection = -1f;
                            ChangeLine();
                            break;
                        }
                    }

                }
                else if (Input.GetKeyDown("d"))
                {
                    for (int i = 0; i < constantsInstance.lines.Length; i++)
                    {
                        if (constantsInstance.IsEqualsFloat(transform.position.y - constantsInstance.lines[i], -6f))
                        {
                            changeOfLineDirection = 1f;
                            ChangeLine();
                            break;
                        }
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
            transform.position += new Vector3(changeOfLineDirection * Constants.epsilon * 0.6f, 0f, 0f);
        }
        else
        {
            startingLine = transform.position.y;
            transform.position += new Vector3(0f, changeOfLineDirection * Constants.epsilon * 0.6f, 0f);
        }


    }

    void SnapToLines()
    {
        int numberOfLine = 0;
        Vector3 direction = Vector3.zero;
        if (gravityDirection == Vector3.down || gravityDirection == Vector3.up)
        {
            for (int i = 0; i < constantsInstance.lines.Length; i++)
            {
                if (constantsInstance.lines[i] == startingLine)
                {
                    continue;
                }

                float tmpDistance = Mathf.Abs(constantsInstance.lines[i] - transform.position.x);
                if (tmpDistance < distanceToLine)
                {
                    distanceToLine = tmpDistance;
                    numberOfLine = i;
                }
            }
            desiredLine.y = 0f;
            desiredLine.x = constantsInstance.lines[numberOfLine];

            if (distanceToLine <= Constants.epsilon * 10)
            {
                Vector3 tempLocation = new Vector3(desiredLine.x - transform.position.x, 0f, 0f);
                transform.position += tempLocation;
                playerRigidbody.velocity = new Vector3(0f, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            else
            {
                direction.x = desiredLine.x - transform.position.x;
                direction = direction.normalized;
                playerRigidbody.velocity = new Vector3(direction.x * changeLineSpeed, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
        }
        else
        {
            for (int i = 0; i < constantsInstance.lines.Length; i++)
            {
                if (constantsInstance.lines[i] == startingLine)
                {
                    continue;
                }

                float tmpDistance = Mathf.Abs(constantsInstance.lines[i] - transform.position.y);
                if (tmpDistance < distanceToLine)
                {
                    distanceToLine = tmpDistance;
                    numberOfLine = i;
                }
            }
            desiredLine.y = constantsInstance.lines[numberOfLine];
            desiredLine.x = 0f;

            if (distanceToLine <= Constants.epsilon * 10)
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
                playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, direction.y * changeLineSpeed, playerRigidbody.velocity.z);
            }
        }
        distanceToLine = 10f;
    }

}