using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    void FixedUpdate ( )
    {
        float speed = GetComponent<Rigidbody2D>( ).velocity.magnitude;
        float angle = GetComponent<Rigidbody2D>( ).rotation;

        if (Input.GetButton("Vertical"))
            GetComponent<Rigidbody2D>( ).velocity = transform.up.normalized * carSpeed * Math.Sign(Input.GetAxisRaw("Vertical"));

        if (Input.GetButton("Jump"))
        {
            speed = speed * decelRate;
            if (speed <= minSpeed)
            {
                GetComponent<Rigidbody2D>( ).angularVelocity = 0;
                speed = 0;
            }
        }

        if (Input.GetButton ("Horizontal"))
            angle = angle + angleIncrement * angleSpeedMultiplier * speed * Math.Sign(-Input.GetAxisRaw("Horizontal"));

        GetComponent<Rigidbody2D>( ).rotation = angle;
    }


    private const float minSpeed = 4f;


    private readonly float angleSpeedMultiplier = 0.39f;
    private readonly float angleIncrement = 1.3f;
    private readonly float decelRate = 0.91f;
    private readonly float carSpeed = 17f;
}