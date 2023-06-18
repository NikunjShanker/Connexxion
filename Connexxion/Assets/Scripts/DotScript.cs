using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotScript : MonoBehaviour
{
    private string name;
    private string type;

    private void Start()
    {
        name = this.transform.gameObject.name;
        type = "white circle";
    }

    private void OnMouseDrag()
    {
        GameplayControllerScript.instance.DotSelected(0, type, name);
    }
}
