using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSquareScript : DotScript
{
    private new void Start()
    {
        base.Start();
        shape = "square";
        color = "red";
    }
}
