﻿using System;
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
    private Quaternion rotateGravityRight = Quaternion.Euler(0, 0, 90);
    private Quaternion rotateGravityLeft = Quaternion.Euler(0, 0, -90);

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
        RaycastHit hit;
        Debug.Log("SphereCast" + Physics.SphereCast(transform.position - new Vector3(0f, 0.4f, 0f), 0.5f, Vector3.down, out hit));
        Debug.Log("Ray" + Physics.Raycast(transform.position - new Vector3(0f, 0.5f, 0f), Vector3.down, 2.5f, LayerMask.GetMask("Floor")));
        Debug.Log("Sphere" + (Physics.OverlapSphere(transform.position - new Vector3(0f, 0.3f, 0f), 0.5f, LayerMask.GetMask("Floor")).Length != 0));
        //Debug.Log(Physics.OverlapSphere(transform.position - new Vector3(0f, 0.3f, 0f), 0.5f, LayerMask.GetMask("Floor")).Length);
        Jump();
        //jumpAvailability = true;
        //ChangeLine();
    }

    void Jump()
    {
        int Layer = LayerMask.GetMask("Floor");
        RaycastHit hit;
        Vector3 halfOfCapsuleColliderExtentsByY = new Vector3(0f, playerCapsuleCollider.bounds.extents.y / 2, 0f);
        if (Physics.SphereCast(transform.position - new Vector3(0f, 0.4f, 0f), 0.5f, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Floor")))
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
            SwitchGravity(rotateGravityLeft); 
        }

        if (Input.GetKeyDown("e"))
        {
            SwitchGravity(rotateGravityRight);       
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
            }
            else if (Input.GetKeyDown("d"))
            {
                changeOfLineDirection = 2f;
            }
            else
                changeOfLineDirection = 0f;
        }
    }
    void ChangeLine()
    {
        if (changeLineAvailability == true & changeOfLineDirection !=0f)
        {
            switch (changeOfLineDirection)
            {
                case 1:
                    playerRigidbody.AddForce(rotateGravityLeft * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                    changeLineAvailability = false;
                    break;
                case 2:
                    playerRigidbody.AddForce(rotateGravityRight * gravityDirection * changeLineSpeed, ForceMode.VelocityChange);
                    changeLineAvailability = false;
                    break;
                default:
                    break;
            }
        }

        

        if (changeLineAvailability == false)
        {
            switch (changeOfLineDirection)
            {
                case 1:
                    playerRigidbody.AddForce(rotateGravityRight * gravityDirection * Mathf.Sqrt((2 * changeLineLength / changeLineTime) - (2 * changeLineSpeed)));
                    break;
                case 2:
                    playerRigidbody.AddForce(rotateGravityLeft * gravityDirection * Mathf.Sqrt((2 * changeLineLength / changeLineTime) - (2 * changeLineSpeed)));
                    break;
                default:
                    break;
            }
        }
    }

    void CheckBorderLine()
    { 
        
    }

}


