using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameplayControllerScript : MonoBehaviour
{
    public static GameplayControllerScript instance;
    public GameObject[] dots;
    public List<GameObject[]> connections;
    public GameObject mouseConnection;
    public GameObject chosenDot;
    public GameObject chosenDot2;

    [SerializeField]
    private GameObject connectionPrefab;

    private bool plausibleLine;
    private bool compatibleConnection;
    
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        getAllDots();

        connections = new List<GameObject[]>();

        mouseConnection = null;
        plausibleLine = true;
        compatibleConnection = true;
    }

    private void Update()
    {
        GetMouse();   
    }

    public void DotSelected(GameObject dot)
    {
        chosenDot = dot;
    }

    public void DotHover(GameObject dot)
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
            if(chosenDot != null && chosenDot2 != null && chosenDot != chosenDot2 && compatibleConnection && checkPreviousConnections(chosenDot, chosenDot2))
            {
                GameObject line = Instantiate(connectionPrefab);
                LineRenderer lineRend = line.GetComponent<LineRenderer>();

                lineRend.gameObject.SetActive(true);
                Vector3 sp = chosenDot.transform.position;
                Vector3 ep = chosenDot2.transform.position;
                ep.z = sp.z;
                lineRend.SetPosition(0, sp);
                lineRend.SetPosition(1, ep);

                PolygonCollider2D collider = lineRend.GetComponent<PolygonCollider2D>();
                collider.points = new[] { new Vector2(sp.x + 0.01f, sp.y + 0.01f), new Vector2(ep.x + 0.01f, ep.y + 0.01f), new Vector2(ep.x - 0.01f, ep.y - 0.01f), new Vector2(sp.x - 0.01f, sp.y - 0.01f) };

                GameObject[] info = new GameObject[3];
                info[0] = lineRend.gameObject;
                info[1] = chosenDot;
                info[2] = chosenDot2;
                connections.Add(info);

                chosenDot.gameObject.GetComponent<DotScript>().AddConnection();
                chosenDot2.gameObject.GetComponent<DotScript>().AddConnection();
            }

            if (mouseConnection != null)
            {
                mouseConnection.SetActive(false);
                mouseConnection.GetComponent<ConnectionScript>().clearCollisions();
            }
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

                if(chosenDot2 != null)
                {
                    DotScript cd1script = chosenDot.GetComponent<DotScript>();
                    DotScript cd2script = chosenDot2.GetComponent<DotScript>();

                    compatibleConnection = checkDotMetadata(cd1script, cd2script) && checkDotMetadata(cd2script, cd1script) && plausibleLine;
                }

                if (plausibleLine && chosenDot2 == null) { line.startColor = Color.grey; line.endColor = Color.grey; }
                else if (compatibleConnection) { line.startColor = Color.white; line.endColor = Color.white; }
                else { line.startColor = Color.red; line.endColor = Color.red; }

                collider.points = new[] { new Vector2(sp.x + 0.01f, sp.y + 0.01f), new Vector2(ep.x + 0.01f, ep.y + 0.01f), new Vector2(ep.x - 0.01f, ep.y - 0.01f), new Vector2(sp.x - 0.01f, sp.y - 0.01f) };
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(connections.Count != 0)
            {
                GameObject[] lastConnection = connections[connections.Count - 1];
                connections.Remove(lastConnection);
                lastConnection[1].GetComponent<DotScript>().RemoveConnection();
                lastConnection[2].GetComponent<DotScript>().RemoveConnection();
                Destroy(lastConnection[0]);
            }
        }
    }

    private bool checkDotMetadata(DotScript d1, DotScript d2)
    {
        if (d1.color == "black" && d2.color != "black") return false;
        else if(d1.color == "blue" && d2.color != "blue" && d2.color != "green" && d2.color != "purple" && d2.color != "white") return false;
        else if(d1.color == "green" && d2.color != "green" && d2.color != "blue" && d2.color != "yellow" && d2.color != "white") return false;
        else if(d1.color == "yellow" && d2.color != "yellow" && d2.color != "green" && d2.color != "orange" && d2.color != "white") return false;
        else if(d1.color == "red" && d2.color != "red" && d2.color != "orange" && d2.color != "purple" && d2.color != "white") return false;
        else if(d1.color == "orange" && d2.color != "orange" && d2.color != "red" && d2.color != "yellow" && d2.color != "white") return false;
        else if(d1.color == "purple" && d2.color != "purple" && d2.color != "red" && d2.color != "blue" && d2.color != "white") return false;

        return true;
    }

    private bool checkPreviousConnections(GameObject d1, GameObject d2)
    {
        for(int i = 0; i < connections.Count; i++)
        {
            int similar = 0;
            GameObject[] conn = connections[i];

            if (conn[1] == d1) similar += 1;
            if (conn[2] == d1) similar += 1;
            if (conn[1] == d2) similar += 1;
            if (conn[2] == d2) similar += 1;
            if (similar >= 2) return false;
        }

        return true;
    }

    private void getAllDots()
    {
        Transform canvas = GameObject.Find("Dots").transform;
        dots = new GameObject[canvas.childCount];
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = canvas.GetChild(i).gameObject;
        }
    }
}
