using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCircleScript : DotScript
{
    private new void Start()
    {
        base.Start();
        shape = "circle";
        color = "black";
    }
}
