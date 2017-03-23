using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartButton: MonoBehaviour, IPointerClickHandler
{
    protected void Start ( )
    {
        GetComponent<Button>( ).onClick.AddListener(ClickThis);
    }

    protected void Update ( )
    {

    }

    protected void FixedUpdate ( )
    {

    }

    public void OnPointerClick (PointerEventData e)
    {
        print("hahaha");
    }

    protected void ClickThis ( )
    {
        //Time.timeScale = 0;
        print("kek");
    }
}