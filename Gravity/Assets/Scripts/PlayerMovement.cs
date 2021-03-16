using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody PlayerRigidbody;

    public float Speed = 5f;
    public float GravForce = 9.8f;
    public float JumpHeight = 3f;

    private Vector3 GravDirection = Vector3.down;
    private bool CanJump = true;
    private Quaternion TargetRotation = Quaternion.identity;
    private Quaternion OldRotation = Quaternion.identity;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchGrav();
    }
    void FixedUpdate()
    {
        // PlayerRigidbody.AddForce();
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Direction = TargetRotation * Direction;
        PlayerRigidbody.MovePosition(transform.position +  Direction * Time.deltaTime * Speed);
        PlayerRigidbody.AddForce(GravDirection * GravForce);
        Jump();
        CanJump = true;
      
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        if (CanJump && Physics.Raycast(transform.position, GravDirection, 1.05f, Layer))
        {
            //Debug.Log(1);
            if (Input.GetKeyDown("space") || Input.GetKey("space"))
            {
                PlayerRigidbody.velocity = Vector3.zero;
                PlayerRigidbody.AddForce(-GravDirection * (float) Math.Sqrt(JumpHeight * (2 * GravForce)), ForceMode.VelocityChange);
               // Debug.Log((float) Math.Sqrt(JumpHeight / (2 * GravForce)));
                CanJump = false;
            }
        }
    }
    void SwitchGrav()
    {
       // if (transform.rotation == TargetRotation * OldRotation)
       // {
            if (Input.GetKeyDown("q"))
            {
                TargetRotation = Quaternion.Euler(0, 0, -90) * TargetRotation;
                GravDirection = TargetRotation * Vector3.down;
              //  OldRotation = transform.rotation;
            }

            if (Input.GetKeyDown("e"))
            {
                TargetRotation = Quaternion.Euler(0, 0, 90) * TargetRotation;
                GravDirection = TargetRotation * Vector3.down;
            //  OldRotation = transform.rotation;
        }
      //  }

        // transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation * OldRotation, Speed * Time.deltaTime * 0.1f) * transform.rotation;
        transform.rotation = TargetRotation * transform.rotation;
    }

}


