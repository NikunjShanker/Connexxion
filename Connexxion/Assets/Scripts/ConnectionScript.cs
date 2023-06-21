using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScript : MonoBehaviour
{
    private bool impossRecognized;
    private bool possRecognized;

    private List<GameObject> dotCollisionObjects;

    private void Start()
    {
        impossRecognized = false;
        possRecognized = true;
        dotCollisionObjects = new List<GameObject>();
    }

    private void Update()
    {
        if(dotCollisionObjects.Count >= 3 && !impossRecognized)
        {
            impossRecognized = true;
            possRecognized = false;
            GameplayControllerScript.instance.ImpossibleLine();
        }
        
        if(dotCollisionObjects.Count < 3 && !possRecognized)
        {
            possRecognized = true;
            impossRecognized = false;
            GameplayControllerScript.instance.PossibleLine();
        }
    }

    public void clearCollisions()
    {
        dotCollisionObjects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Circle")
        {
            if(!dotCollisionObjects.Contains(collision.gameObject))
            {
                dotCollisionObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Circle")
        {
            dotCollisionObjects.Remove(collision.gameObject);
        }
    }
}
