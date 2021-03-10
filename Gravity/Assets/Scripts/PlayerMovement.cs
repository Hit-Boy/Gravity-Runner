using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;

    private int GravState = 1;
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
            if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
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

        character.Move(Direction * Time.deltaTime);

    }
    void FixedUpdate()
    {
        CanJump = true;
    }

}

    