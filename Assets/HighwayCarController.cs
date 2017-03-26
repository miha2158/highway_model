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
        Speed = Direction * StartSpeed;
        if (name != "GOCar")
            CarStops = R.Next(0, 100) <= SlowChance;

        if(CarStops)
        {
            //transform.localScale = new Vector3(6f,3f);
        }
    }

    protected void FixedUpdate ( )
    {
        if (Body.position.x >= CarSpawner.despawnPosition)
            Destroy(gameObject);

        Cycle++;

        if ((Stopping || Cycle >= UncheckCycle) && Velocity <= ThresholdSpeed)
        {
            if (Cycle == UncheckCycle)
            {
                Stopping = false;
            }
            if (Cycle > UncheckCycle)
                Cycle = 0;
        }

        if (CarStops)
        {
            if (Stopping || (Cycle % CheckCycle == 0 && R.Next(100) <= SlowChance))
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
            Speed = Speed.normalized * Math.Abs(Velocity - SpeedV3);
        else
            Speed = Speed.normalized * ThresholdSpeed;
    }
    protected void Accelerate (bool positive)
    {
        Speed = positive
            ? (Velocity >= MinSpeed
                ? Velocity >= MaxSpeed
                    ? Speed.normalized * MaxSpeed
                    : (Speed.normalized + Direction).normalized * (Velocity + SpeedV1)

                : (Speed.magnitude >= ThresholdSpeed
                    ? (Speed.normalized + Direction).normalized * Velocity * SpeedV2
                    : Speed.normalized * ThresholdSpeed))

            : (Velocity > ThresholdSpeed
                ? Speed.normalized * (Velocity - SpeedV2)
                : Speed.normalized * ThresholdSpeed);


        Speed = Velocity > MaxSpeed
            ? Speed.normalized * MaxSpeed
            : Speed;
    }
    protected void OnMouseDown ( )
    {
        Body.AddForce(Direction * 50000);
    }
    
    protected bool Obstacle ( )
    {
        return Physics2D.RaycastAll(Body.position, Direction, Math.Max((Velocity + 1) * DistanceMultiplier1, MinDistance1)).Where(p => p.rigidbody != Body).Any( );
            //Any(p => p.distance <= MinDistance1 || p.distance < Velocity * DistanceMultiplier1 + MinDistance1);
    }
    protected void OnCollisionEnter2D (Collision2D c)
    {
        if (name.Contains("(Clone)"))
        {
            Time.timeScale = 0;
            GameObject.Find("Pause").GetComponentInChildren<Text>( ).text = "Продолжить";
        }
    }

    #region Stops

    public static int SlowChance = 23;
    public static readonly int SlowChanceDefault = SlowChance;
    protected static int StopChance = 25;
    protected static ushort CheckCycle = 60;

    protected static System.Random R = new System.Random( );
    protected static uint UncheckCycle = 50;
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

    protected static float ThresholdSpeed = 5f;
    protected static float MinSpeed = 30f;
    protected static float StartSpeed = 65f;
    public static float MaxSpeed = 100f;
    public static readonly float MaxSpeedDefault = MaxSpeed;

    #endregion 

    #region Statics

    protected static float SpeedV1 = 3f;
    protected static float SpeedV2 = 4.5f;
    protected static float SpeedV3 = 8f;

    public static float MinDistance1 = 15f;
    protected static float DistanceMultiplier1 = 0.55f;// 3.6f;

    #endregion
}