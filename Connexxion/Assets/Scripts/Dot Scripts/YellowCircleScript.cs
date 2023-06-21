using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCircleScript : DotScript
{
    private void Start()
    {
        base.Start();
        shape = "circle";
        color = "yellow";
    }
}
