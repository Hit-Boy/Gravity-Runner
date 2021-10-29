using System;
using System.Net;
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

    private Quaternion desiredRotation = Quaternion.identity;
    private Quaternion actualRotation = Quaternion.identity;
    private Vector3 gravityDirection = Vector3.down;

    private int desiredLaneIndex = 1;
    private KeyCode keyStored = KeyCode.None;
    private int state = 0; // 0 - idle, 1 - move, 2 - switch gravity
    private int previousState = 0;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<Collider>();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        playerRigidbody.AddForce(gravityDirection * gravityForce);
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
            var tmpKey = keyStored;
            if (OnGroundCheck() && keyStored == KeyCode.Space)
            {
                keyStored = KeyCode.None;

                CheckInputConditions(tmpKey);
                return;
            }

            if (previousState == 0 && !OnGroundCheck()) return;
            previousState = 0;
            if (keyStored == KeyCode.None)
                return;
            keyStored = KeyCode.None;

            CheckInputConditions(tmpKey);
        }

        if (state == 1)
            PullToLane();

        if (state == 2)
            RotateToGravity();
    }

    private void PullToLane()
    {
        if (Constants.IsEqualVector(gravityDirection, Vector3.down) ||
            Constants.IsEqualVector(gravityDirection, Vector3.up))
        {
            if (Math.Abs(Constants.Lanes[desiredLaneIndex] - transform.position.x) <=
                changeLaneSpeed * Time.fixedDeltaTime)
            {
                transform.position = new Vector3(Constants.Lanes[desiredLaneIndex], transform.position.y,
                    transform.position.z);
                state = 0;
            }
            else
            {
                var moveDirection = new Vector3(Constants.Lanes[desiredLaneIndex] - transform.position.x, 0,
                    0);
                transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (Mathf.Abs(Constants.Lanes[desiredLaneIndex] - transform.position.y) <=
                changeLaneSpeed * Time.fixedDeltaTime)
            {
                transform.position = new Vector3(transform.position.x, Constants.Lanes[desiredLaneIndex],
                    transform.position.z);
                state = 0;
            }
            else
            {
                var moveDirection = new Vector3(0f, Constants.Lanes[desiredLaneIndex] - transform.position.y,
                    0f);
                transform.position += moveDirection.normalized * changeLaneSpeed * Time.fixedDeltaTime;
            }
        }

        previousState = 1;
    }

    private void RotateToGravity()
    {
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, desiredRotation, rotateSpeed * Time.fixedDeltaTime);
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
        if (Constants.IsEqualVector(gravityDirection, Vector3.down) ||
            Constants.IsEqualVector(gravityDirection, Vector3.left))
        {
            if (desiredLaneIndex + direction == Constants.Lanes.Length ||
                desiredLaneIndex + direction == -1)
                return;
            desiredLaneIndex += direction;
        }
        else
        {
            if (desiredLaneIndex - direction == Constants.Lanes.Length ||
                desiredLaneIndex - direction == -1)
                return;
            desiredLaneIndex -= direction;
        }
    }

    private void ChangeGravityDirection(Quaternion rotationDirection)
    {
        gravityDirection = rotationDirection * gravityDirection;
        desiredRotation = rotationDirection * desiredRotation;
        FindDesiredLaneIndex();
    }

    //Input methods
    private void CheckInputConditions(KeyCode keyPressed)
    {
        if (keyStored == KeyCode.Space)
            keyStored = KeyCode.None;

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
                    ChangeGravityDirection(Quaternion.Euler(0f, 0f, -90f));
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
                    ChangeGravityDirection(Quaternion.Euler(0f, 0f, 90f));
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
                Debug.Log(keyStored);
                Debug.Log(OnGroundCheck());
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
    private void FindDesiredLaneIndex()
    {
        if (Constants.IsEqualVector(gravityDirection, Vector3.down) ||
            Constants.IsEqualVector(gravityDirection, Vector3.up))
        {
            var distanceToLane = 10f;
            for (var i = 0; i < Constants.Lanes.Length; i++)
            {
                var tmpDistance = Mathf.Abs(Constants.Lanes[i] - transform.position.x);
                if (!(tmpDistance <= distanceToLane)) continue;
                distanceToLane = tmpDistance;
                desiredLaneIndex = i;
            }
        }
        else
        {
            var distanceToLane = 10f;
            for (var i = 0; i < Constants.Lanes.Length; i++)
            {
                var tmpDistance = Mathf.Abs(Constants.Lanes[i] - transform.position.y);
                if (!(tmpDistance <= distanceToLane)) continue;
                distanceToLane = tmpDistance;
                desiredLaneIndex = i;
            }
        }
    }

    private bool OnGroundCheck()
    {
        var layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        var halfOfCapsuleColliderExtentsByYMinusEpsilon =
            new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f) -
            new Vector3(0f, 10 * Constants.Epsilon, 0f);
        return Physics.SphereCast(transform.position - halfOfCapsuleColliderExtentsByYMinusEpsilon,
            playerCapsuleCollider.bounds.extents.x, gravityDirection, out hit, 10 * Constants.Epsilon, layer);
    }
}