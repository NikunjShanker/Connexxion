using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScript : MonoBehaviour
{
    public bool impossRecognized;
    public bool possRecognized;

    public List<GameObject> dotCollisionObjects;

    public void Start()
    {
        impossRecognized = false;
        possRecognized = true;
        dotCollisionObjects = new List<GameObject>();
    }

    public void Update()
    {
        checkCollisions();
    }

    public void clearCollisions()
    {
        dotCollisionObjects.Clear();
    }

    public void checkCollisions()
    {
        if (dotCollisionObjects.Count >= 3 && !impossRecognized)
        {
            impossRecognized = true;
            possRecognized = false;
            GameplayControllerScript.instance.ImpossibleLine();
        }

        if (dotCollisionObjects.Count < 3 && !possRecognized)
        {
            possRecognized = true;
            impossRecognized = false;
            GameplayControllerScript.instance.PossibleLine();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Circle" || collision.tag == "Square")
        {
            if (!dotCollisionObjects.Contains(collision.gameObject))
            {
                dotCollisionObjects.Add(collision.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Circle" || collision.tag == "Square")
        {
            dotCollisionObjects.Remove(collision.gameObject);
        }
    }
}
