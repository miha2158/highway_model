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
        Speed = Direction * StartSpeed;

        CarStops = R.Next(0, 100) <= SlowChance;
    }

    protected void Update ( )
    {

    }

    protected void FixedUpdate ( )
    {
        if (Body.position.x >= CarSpawner.despawnPosition)
            Destroy(gameObject);




        if (CarStops)
        {
            Cycle++;

            if (Stopping && Velocity <= ThresholdSpeed)
            {
                if (Cycle == UncheckCycle)
                {
                    CarStops = false;
                    Stopping = false;
                }
                if (Cycle >= UncheckCycle)
                    Cycle = 0;
            }
            else if (Stopping || (Cycle % CheckCycle == 0 && R.Next(0, 100) <= SlowChance))
            {
                RDecelerate( );
                if (!Stopping)
                {
                    Stopping = true;
                    Cycle = 0;
                }
            }
        }

        if (!Stopping)
            Accelerate(!Obstacle( ));

    }

    protected void RDecelerate ( )
    {
        if (Velocity > ThresholdSpeed)
            Speed = Speed.normalized * Math.Abs(Velocity - SpeedV3 * Deceleration);
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
            Where(p => p.rigidbody != Body).Any(p => p.distance <= MinimumDistance1 || p.distance < Velocity * DistanceMultiplier1);
    }

    public void OnMouseDown ( )
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void OnCollisionEnter (Collision c)
    {
        Time.timeScale = 0;
    }

    #region Stops

    public static int SlowChance = 35;
    protected static int StopChance = 25;
    protected static ushort CheckCycle = 30;

    protected static System.Random R = new System.Random( );
    protected static uint UncheckCycle = 25;
    protected uint Cycle = 0;
    protected bool Stopping = false;
    protected bool CarStops = false;

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

    public static float ThresholdSpeed = RunSpeed * 5f;
    public static float MinSpeed = RunSpeed * 40f;
    public static float StartSpeed = RunSpeed * 70f;
    public static float MaxSpeed = RunSpeed * 100f;

    #endregion 

    #region Statics

    public static float SpeedV1 = RunSpeed * 2.5f;
    public static float SpeedV2 = RunSpeed * 4f;
    public static float SpeedV3 = RunSpeed * 15f;

    public static float MinimumDistance1 = 15f;
    public static float DistanceMultiplier1 = 1.5f / 3.6f;

    #endregion
}