using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;

    private int GravState = 1; // 0 is left, 1 is middle, 2 is right
    private int GravChange = 2; //1 is right, -1 is left, 2 is no change
    private float DirectionY = 0f;
    private float DirectionX = 0f;
    private float DirectionZ = 0f;
    private bool CanJump = true;

    public float SideSpeed = 8f;
    public float ForwardSpeed = 10f;
    public float Grav = 9.8f;
    public float JumpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        DirectionX = Input.GetAxis("Horizontal");
        DirectionZ = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(DirectionX, DirectionY, DirectionZ);
        if (character.isGrounded && CanJump == true)
        {
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                DirectionY += JumpSpeed;
                CanJump = false;
            }
        }

        DirectionY -= Grav * Time.deltaTime;

        if (character.isGrounded && CanJump == true)
        {
            DirectionY = 0f;    
        }

        Direction.y = DirectionY;
        Direction.x *= SideSpeed;
        Direction.z *= SideSpeed;

        if (Input.GetKeyDown("q"))
        {
            GravChange = 0;
            print(GravChange);
        }

        if (Input.GetKeyDown("e"))
        {
            GravChange = 1;
            print(GravChange);
        }


        switch (GravChange)
        {
            case 0:
                switch (GravState)
                {
                    case 1: 
                        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right);
                        GravState--;
                        GravChange = 2;
                        break;

                    case 2:
                        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
                        GravState--;
                        GravChange = 2;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (GravState)
                {
                    case 0:
                        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
                        GravState++;
                        GravChange = 2;
                        break;
                    case 1:
                        transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.left);
                        GravState++;
                        GravChange = 2;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        switch (GravState)
        {
            case 0:
                Direction = Quaternion.FromToRotation(Vector3.up, Vector3.right) * Direction;
                break;
            case 2:
                Direction = Quaternion.FromToRotation(Vector3.up, Vector3.left) * Direction;
                break;
            default:
                break;
        }
        character.Move(Direction * Time.deltaTime);

    }
    void FixedUpdate()
    {
        CanJump = true;
    }

}

    