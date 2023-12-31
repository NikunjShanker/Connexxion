﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareConnectionScript : ConnectionScript
{
    public List<GameObject> connectionsCrossed;
    private Color32[] retainedColor;
    private bool crossRecognized;
    private bool uncrossRecognized;

    private new void Start()
    {
        base.Start();
        connectionsCrossed = new List<GameObject>();
        retainedColor = new Color32[2];
        crossRecognized = false;
        uncrossRecognized = true;
    }

    private new void Update()
    {
        checkCollisions();
    }

    private new void checkCollisions()
    {
        base.checkCollisions();

        if (connectionsCrossed.Count > 0 && !crossRecognized)
        {
            crossRecognized = true;
            uncrossRecognized = false;
            retainedColor[0] = this.GetComponent<LineRenderer>().startColor;
            retainedColor[1] = this.GetComponent<LineRenderer>().endColor;
            this.GetComponent<LineRenderer>().startColor = GameplayControllerScript.instance.incorrectColor; this.GetComponent<LineRenderer>().endColor = GameplayControllerScript.instance.incorrectColor;
        }
        else if (connectionsCrossed.Count == 0 && !uncrossRecognized)
        {
            crossRecognized = false;
            uncrossRecognized = true;
            this.GetComponent<LineRenderer>().startColor = retainedColor[0]; this.GetComponent<LineRenderer>().endColor = retainedColor[1];
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Connection")
        {
            if(!connectionsCrossed.Contains(collision.gameObject))
            {
                connectionsCrossed.Add(collision.gameObject);
            }
        }
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(collision.tag == "Connection")
        {
            connectionsCrossed.Remove(collision.gameObject);
        }
    }
}
