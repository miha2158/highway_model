using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HighwayCarController: MonoBehaviour
{
    protected void Start ( )
    {
        //Instantiate(gameObject, new Vector3(gameObject.transform.position.x - R.Next(20,60), R.Next(2) * (-10)), transform.rotation);
        Speed = Direction * StartSpeed * R.Next(4, 11) / 10;
    }

    protected void Update ( )
    {

    }

    protected void FixedUpdate ( )
    {
        Cycle++;

        if (Stopping && Velocity < ThresholdSpeed && Cycle == UncheckCycle)
        {
            Cycle = 0;
            Stopping = false;
        }
        else if (Cycle % CheckCycle == 0 && R.Next(0, 100) < SlowChance)
        {
            //RDecelerate( );
            Speed = Vector2.zero;
            Stopping = true;
            Cycle = 0;
        }

        Accelerate(!Obstacle( ) && !Stopping);

        if (Body.position.x >= CarSpawner.despawnPosition)
            Destroy(gameObject);

    }

    protected void RDecelerate ( )
    {
        if (Velocity > ThresholdSpeed)
            Speed = Speed.normalized * (Velocity - SpeedV3 * Deceleration);
        else
            Speed = Vector2.zero;
    }

    protected void Accelerate (bool positive)
    {
        Speed = positive
            ? (Velocity > MinSpeed
                ? Velocity >= MaxSpeed
                    ? Speed.normalized * MaxSpeed
                    : (Speed.normalized + Direction).normalized * (Velocity + SpeedV1 * Acceleration)
                : (Speed == Vector2.zero
                    ? Direction * RunSpeed * Acceleration
                    : (Speed.normalized + Direction).normalized * Velocity * SpeedV2 * Acceleration))

            : (Velocity > (MinSpeed + MaxSpeed) / 2
                ? Speed.normalized * (Velocity - SpeedV1 * Deceleration)
                : (Velocity > ThresholdSpeed
                    ? Speed.normalized * (Velocity - SpeedV2 * Deceleration)
                    : Vector2.zero));


        Speed = Velocity > MaxSpeed
            ? Speed.normalized * MaxSpeed
            : Speed;
    }

    protected bool Obstacle ( )
    {
        return Physics2D.RaycastAll(Body.position, Direction, Speed.magnitude * (DistanceMultiplier1 + MinSpeed)).
            Where(p => p.rigidbody != Body).Any(p => p.distance <= MinimumDistance1 || p.distance < Speed.magnitude * DistanceMultiplier1);
    }

    protected void OnMouseDown ( )
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    #region Stops

    public static int SlowChance = 35;
    protected ushort CheckCycle = 25;

    protected static System.Random R = new System.Random( );
    protected uint Cycle = 0;
    protected uint UncheckCycle = 5;
    protected bool Stopping = false;

    #endregion

    #region Shortcuts

    protected Rigidbody2D Body
    {
        get { return GetComponent<Rigidbody2D>( ); }
    }
    protected Vector2 Speed
    {
        get { return Body.velocity; }
        set { Body.velocity = value; }
    }
    protected float Velocity
    {
        get { return Speed.magnitude; }
    }
    protected Vector2 Direction
    {
        get { return new Vector2(-(float)Math.Sin(Body.rotation * Math.PI / 180), (float)Math.Cos(Body.rotation * Math.PI / 180)).normalized; }
        set { Body.rotation = -Vector2.Angle(Vector2.up, value); }
    }

    #endregion

    #region Speeds

    public static float Acceleration = 1f;
    public static float Deceleration = 1f;

    public static float RunSpeed = 1f;

    public static float ThresholdSpeed = RunSpeed * 4f;
    public static float MinSpeed = RunSpeed * 40f;
    public static float StartSpeed = RunSpeed * 80f;
    public static float MaxSpeed = RunSpeed * 130f;

    #endregion 

    #region Statics

    public static float SpeedV1 = RunSpeed * 2.5f;
    public static float SpeedV2 = RunSpeed * 4f;
    public static float SpeedV3 = RunSpeed * 27f;

    public static float MinimumDistance1 = 15f;
    public static float DistanceMultiplier1 = 1.5f / 3.6f;

    #endregion
}