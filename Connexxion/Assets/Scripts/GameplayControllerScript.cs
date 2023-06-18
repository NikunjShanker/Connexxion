using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayControllerScript : MonoBehaviour
{
    public static GameplayControllerScript instance;
    public GameObject[] dots;

    private Camera cam;
    
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        GetAllDots();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        
    }

    public void DotSelected(int order, string type, string name)
    {
        Debug.Log(name);
    }

    private void GetAllDots()
    {
        Transform canvas = GameObject.Find("Dots Canvas").transform;
        dots = new GameObject[canvas.childCount];
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = canvas.GetChild(i).gameObject;
        }
    }
}
