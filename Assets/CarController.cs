using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    //initialisation
    void Start ( )
    {
        /*angle = GetComponent<Rigidbody2D> ().rotation;
		speed = GetComponent<Rigidbody2D> ().velocity.magnitude;*/
		
    }

    //once per frame
    void Update ( )
    {
		
    }

    //once per game engine tick
    void FixedUpdate ( )
    {
        //Rigidbody2D Car = GetComponent<Rigidbody2D> ( );
        float speed = GetComponent<Rigidbody2D>( ).velocity.magnitude;
        float angle = GetComponent<Rigidbody2D>( ).rotation;

        if (Input.GetButton("Accelerate"))
            GetComponent<Rigidbody2D>( ).AddForce(transform.up * carSpeed);

        if (Input.GetButton("Decelerate"))
            GetComponent<Rigidbody2D>( ).AddForce(transform.up * -carSpeed);

        if (Input.GetButton("Brake"))
        {
            speed = speed * decelRate;
            if (speed <= minSpeed)
            {
                GetComponent<Rigidbody2D>( ).angularVelocity = 0;
                speed = 0;
            }
        }

        if (Input.GetButton ("Left"))
            angle = angle + angleIncrement * angleSpeedMultiplier * speed;

        if (Input.GetButton ("Right"))
            angle = angle - angleIncrement * angleSpeedMultiplier * speed;


        GetComponent<Rigidbody2D>( ).rotation = angle;
        //GetComponent<Rigidbody2D>( ).velocity = GetComponent<Rigidbody2D>( ).velocity.normalized * speed;

    }


    private const float minSpeed = 4f;


    private readonly float angleSpeedMultiplier = 0.39f;
    private readonly float angleIncrement = 2.25f;
    //private readonly float defaultDecelRate = 0.3f;
    private readonly float decelRate = 0.91f;
    private readonly float carSpeed = 4.6f;

}