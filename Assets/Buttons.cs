using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons: MonoBehaviour
{
    public void Press1 ( )
    {
        //print("kek");
        var DeleteList = FindObjectsOfType<GameObject>( ).Where(i => i.name.Contains("(Clone)"));
        foreach (var o in DeleteList)
            Destroy(o);
    }

    public void Press2 ( )
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}