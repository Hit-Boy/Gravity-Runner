using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody PlayerRigidbody;

    public float Speed = 5f;
    public float Grav = 9.8f;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        // PlayerRigidbody.AddForce();
        Vector3 Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Debug.Log(Direction);
        PlayerRigidbody.MovePosition(transform.position + Direction *Time.deltaTime * Speed);
    }
}

