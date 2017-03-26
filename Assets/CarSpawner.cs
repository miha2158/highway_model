using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Boo.Lang.Runtime.DynamicDispatching;
using UnityEngine;
using UnityEngine.UI;

public class CarSpawner: MonoBehaviour
{
    protected void FixedUpdate ( )
    {
        if (Cycle % R.Next(CheckCycle, CheckCycle * 2) == 0)
        {
            var newPos = new Vector3(spawnPosition, (R.Next(lanesCount) - 1) * (-10));
            if (!Physics2D.OverlapCircleAll(newPos, 10).Any( ))
                Instantiate(GOCar, newPos, Quaternion.Euler(0, 0, -90));
            Cycle = 0;
        }

        Cycle++;
    }



    protected uint Cycle = 0;
    protected int CheckCycle = 20/lanesCount;
    public static byte lanesCount = 1;


    public static int spawnPosition = -220;
    public static int despawnPosition = -spawnPosition;
    protected static System.Random R = new System.Random( );
    public GameObject GOCar;
    public static int waitTime = 300;
}