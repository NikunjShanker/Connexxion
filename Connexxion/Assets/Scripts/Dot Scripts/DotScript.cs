using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DotScript : MonoBehaviour
{
    public string shape;
    public string color;
    public int requiredConnNum;
    private bool recognized;

    private TextMeshProUGUI text;

    public void Start()
    {
        shape = "";
        color = "";
        recognized = false;

        text = this.GetComponentInChildren<TextMeshProUGUI>();
        int.TryParse(text.text, out requiredConnNum);
    }

    private void Update()
    {
        if (requiredConnNum == 0) text.text = "";
        else text.text = requiredConnNum.ToString();
    }

    public void AddConnection()
    {
        requiredConnNum -= 1;
    }

    public void RemoveConnection()
    {
        requiredConnNum += 1;
    }

    private void OnMouseDrag()
    {
        if(!recognized)
        {
            GameplayControllerScript.instance.DotSelected(this.gameObject);
            recognized = true;
        }
    }

    private void OnMouseEnter()
    {
        if(GameplayControllerScript.instance.chosenDot2 != this.gameObject)
        {
            GameplayControllerScript.instance.DotHover(this.gameObject);
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