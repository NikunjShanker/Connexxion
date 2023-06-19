using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameplayControllerScript : MonoBehaviour
{
    public static GameplayControllerScript instance;
    public GameObject[] dots;
    public List<LineRenderer> connections;
    public GameObject mouseConnection;
    public GameObject chosenDot;
    public GameObject chosenDot2;

    [SerializeField]
    private GameObject connectionPrefab;

    private bool plausibleLine;
    
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        GetAllDots();

        mouseConnection = null;
        plausibleLine = true;
    }

    private void Update()
    {
        GetMouse();   
    }

    public void DotSelected(string type, GameObject dot)
    {
        chosenDot = dot;
    }

    public void DotHover(string type, GameObject dot)
    {
        chosenDot2 = dot;
    }

    public void DotUnHover()
    {
        chosenDot2 = null;
    }

    public void ImpossibleLine()
    {
        plausibleLine = false;
    }

    public void PossibleLine()
    {
        plausibleLine = true;
    }

    private void GetMouse()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(chosenDot != null && chosenDot2 != null && plausibleLine)
            {
                GameObject line = Instantiate(connectionPrefab);
                LineRenderer lineRend = line.GetComponent<LineRenderer>();

                lineRend.gameObject.SetActive(true);
                Vector3 sp = chosenDot.transform.position;
                Vector3 ep = chosenDot2.transform.position;
                lineRend.SetPosition(0, sp);
                lineRend.SetPosition(1, ep);

                PolygonCollider2D collider = lineRend.GetComponent<PolygonCollider2D>();
                collider.points = new[] { new Vector2(sp.x + 0.1f, sp.y + 0.1f), new Vector2(ep.x + 0.1f, ep.y + 0.1f), new Vector2(ep.x - 0.1f, ep.y - 0.1f), new Vector2(sp.x - 0.1f, sp.y - 0.1f) };

                connections.Add(lineRend);
            }

            if (mouseConnection != null) mouseConnection.SetActive(false);
            chosenDot = null;
            chosenDot2 = null;
        }
        else if (Input.GetMouseButton(0))
        {
            if(chosenDot != null)
            {
                if (mouseConnection == null) mouseConnection = Instantiate(connectionPrefab); mouseConnection.tag = "Mouse Connection";

                LineRenderer line = mouseConnection.GetComponent<LineRenderer>();
                PolygonCollider2D collider = mouseConnection.GetComponent<PolygonCollider2D>();

                mouseConnection.gameObject.SetActive(true);
                Vector3 sp = chosenDot.transform.position;
                Vector3 ep = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ep.z = sp.z;

                line.SetPosition(0, sp);
                line.SetPosition(1, ep);

                if (plausibleLine && chosenDot2 == null) { line.startColor = Color.grey; line.endColor = Color.grey; }
                else if (plausibleLine) { line.startColor = Color.white; line.endColor = Color.white; }
                else { line.startColor = Color.red; line.endColor = Color.red; }

                collider.points = new[] { new Vector2(sp.x + 0.1f, sp.y + 0.1f), new Vector2(ep.x + 0.1f, ep.y + 0.1f), new Vector2(ep.x - 0.1f, ep.y - 0.1f), new Vector2(sp.x - 0.1f, sp.y - 0.1f) };
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(connections.Count != 0)
            {
                LineRenderer lastConnection = connections[connections.Count - 1];
                connections.Remove(lastConnection);
                Destroy(lastConnection.gameObject);
            }
        }
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
