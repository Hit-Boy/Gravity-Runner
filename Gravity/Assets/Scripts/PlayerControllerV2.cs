using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    Rigidbody playerRigidbody;
    Collider playerCapsuleCollider;
    Constants constantsInstance = new Constants();  

    [SerializeField]
    private float forwardSpeed = 5f;
    [SerializeField]
    private float gravityForce = 9.8f;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float changeLaneSpeed = 25f;
    [SerializeField]
    private float rotateSpeed = 25f;

    private float desiredLane = 0f;
    private bool pullNeeded = false;
    private bool rotateNeeded = false;
    private bool jumpAvailability = false;
    private Vector3 gravityDirection = Vector3.down;
    Quaternion desiredRotation = Quaternion.identity;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void PullToLane()
    {
        if (constantsInstance.IsEqualVector(gravityDirection, Vector3.down) || constantsInstance.IsEqualVector(gravityDirection, Vector3.up))
        {
            float directionToDesiredLane = Mathf.Sign(desiredLane - transform.position.x);
            playerRigidbody.velocity = new Vector3(directionToDesiredLane * changeLaneSpeed * Time.deltaTime, playerRigidbody.velocity.y, playerRigidbody.velocity.z);

            float tmpDistanceToLine = Mathf.Abs(desiredLane - transform.position.x);
            if (tmpDistanceToLine <= Constants.epsilon * 10f)
            {
                transform.position = new Vector3(desiredLane, transform.position.y, transform.position.z);
            }
        }
        else
        {
            float directionToDesiredLane = Mathf.Sign(desiredLane - transform.position.y);
            playerRigidbody.velocity = new Vector3(transform.position.x, directionToDesiredLane * changeLaneSpeed * Time.deltaTime, playerRigidbody.velocity.z);

            float tmpDistanceToLine = Mathf.Abs(desiredLane - transform.position.y);
            if (tmpDistanceToLine <= Constants.epsilon * 10f)
            {
                transform.position = new Vector3(transform.position.x, desiredLane, transform.position.z);
            }
        }
    }

    void RotateToGravity()
    {   
        Quaternion.RotateTowards(transform.rotation, desiredRotation, rotateSpeed);
    }

    void FindDesiredLane()
    {
        if (constantsInstance.IsEqualVector(gravityDirection, Vector3.down) || constantsInstance.IsEqualVector(gravityDirection, Vector3.up))
        {
            float distanceToLane = 10f;
            int numberOfDesiredLane = 3;
            for (int i = 0; i < constantsInstance.lines.Length; i++)
                {
                float tmpDistance = Mathf.Abs(constantsInstance.lines[i] - transform.position.x);
                if (tmpDistance < distanceToLane)
                {
                    distanceToLane = tmpDistance;
                    numberOfDesiredLane = i;
                }
            }
            desiredLane = constantsInstance.lines[numberOfDesiredLane];
        }
        else
        {
            float distanceToLane = 10f;
            int numberOfDesiredLane = 3;
            for (int i = 0; i < constantsInstance.lines.Length; i++)
                {
                float tmpDistance = Mathf.Abs(constantsInstance.lines[i] - transform.position.y);
                if (tmpDistance < distanceToLane)
                {
                    distanceToLane = tmpDistance;
                    numberOfDesiredLane = i;
                }
            }
            desiredLane = constantsInstance.lines[numberOfDesiredLane];
        }
    }




}
