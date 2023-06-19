using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScript : MonoBehaviour
{
    private bool recognized;

    private void Start()
    {
        recognized = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!recognized && collision.tag == "Connection")
        {
            GameplayControllerScript.instance.ImpossibleLine();
            recognized = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (recognized && collision.tag == "Connection")
        {
            GameplayControllerScript.instance.PossibleLine();
            recognized = false;
        }
    }
}
