using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCircleScript : DotScript
{
    private new void Start()
    {
        base.Start();
        shape = "circle";
        color = "red";
    }
}
