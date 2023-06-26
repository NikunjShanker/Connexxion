using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonScript : MonoBehaviour
{
    public void RestartLevel()
    {
        GameplayControllerScript.instance.Reset();
    }
}
