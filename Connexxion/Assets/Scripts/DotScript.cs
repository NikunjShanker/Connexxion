using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotScript : MonoBehaviour
{
    private string type;
    private bool recognized;

    private TextMeshProUGUI text;

    [SerializeField]
    public int requiredConnNum;

    private void Start()
    {
        type = "white circle";
        recognized = false;

        text = this.GetComponentInChildren<TextMeshProUGUI>();
        int.TryParse(text.text, out requiredConnNum);
    }

    private void OnMouseDrag()
    {
        if(!recognized)
        {
            GameplayControllerScript.instance.DotSelected(type, this.gameObject);
            recognized = true;
        }
    }

    private void OnMouseEnter()
    {
        if(GameplayControllerScript.instance.chosenDot2 != this.gameObject)
        {
            GameplayControllerScript.instance.DotHover(type, this.gameObject);
        }
    }

    private void OnMouseExit()
    {
        if (GameplayControllerScript.instance.chosenDot2 != null)
        {
            GameplayControllerScript.instance.DotUnHover();
        }
    }

    private void OnMouseUp()
    {
        recognized = false;
    }
}
