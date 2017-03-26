using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons: MonoBehaviour
{
    protected void Start ( )
    {
        Time.fixedDeltaTime *= 0.85f;
        C1.gameObject.SetActive(canvas);
        C2.gameObject.SetActive(!canvas);
        Continue( );
    }


    public void Pause ( )
    {
        Time.timeScale = 0;
        PauseButton.GetComponentInChildren<Text>( ).text = "Продолжить";
    }
    public void Continue ( )
    {
        Time.timeScale = Speed;
        PauseButton.GetComponentInChildren<Text>( ).text = "Пауза";
    }

    public void Press1 ( )
    {
        Pause( );
        var DeleteList = FindObjectsOfType<GameObject>( ).Where(i => i.name.Contains("GOCar(Clone)"));
        foreach (var o in DeleteList)
            Destroy(o);
    }
    public void Press2 ( )
    {
        if (Time.timeScale == 0)
        {
            Continue( );
        }
        else
        {
            Pause( );
        }
    }
    public void Press3 ( )
    {
        Pause( );
        canvas = !canvas;
        C1.gameObject.SetActive(canvas);
        C2.gameObject.SetActive(!canvas);
    }

    public void Slide1 ( )
    {
        S1.GetComponentsInChildren<Text>( ).FirstOrDefault(i => i.name == "Value").text = ((int)S1.value).ToString();

        Speed = S1.value / 100;
    }
    public void Slide2 ( )
    {
        S2.GetComponentsInChildren<Text>( ).FirstOrDefault(i => i.name == "Value").text = ((int)S2.value).ToString( );

        HighwayCarController.SlowChance = (int)S2.value;
    }
    public void Slide3 ( )
    {
        S3.GetComponentsInChildren<Text>( ).FirstOrDefault(i => i.name == "Value").text = ((int)S3.value).ToString( );

        HighwayCarController.MaxSpeed = S3.value;
    }

    public void ResetButton ( )
    {
        S1.value = 100;
        S2.value = HighwayCarController.SlowChanceDefault;
        S3.value = HighwayCarController.MaxSpeedDefault;
        Press1( );
    }

    public Slider S1;
    public Slider S2;
    public Slider S3;
    public Button PauseButton;
    public Canvas C1;
    public Canvas C2;
    protected bool canvas = true;
    public static float Speed = 1f;

}