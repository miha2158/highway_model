using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Boo.Lang.Runtime.DynamicDispatching;
using UnityEngine;
using UnityEngine.UI;

public class CarSpawner: MonoBehaviour
{

    protected void Start ( )
    {
    }

    protected void Update ( )
    {

    }

    protected void FixedUpdate ( )
    {
        if (Cycle % R.Next(CheckCycle, CheckCycle * 2) == 0)
        {
            Instantiate(GOCar, new Vector3(spawnPosition, R.Next(2) * (-10)), Quaternion.Euler(0, 0, -90));
            Cycle = 0;
        }

        Cycle++;
    }



    protected uint Cycle = 0;
    protected ushort CheckCycle = 15;


    public static int spawnPosition = -350;
    public static int despawnPosition = 350;
    protected static System.Random R = new System.Random( );
    public GameObject GOCar;
    public static int waitTime = 300;
}