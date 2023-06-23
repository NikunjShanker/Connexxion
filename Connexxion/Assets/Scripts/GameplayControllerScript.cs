﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameplayControllerScript : MonoBehaviour
{
    public static GameplayControllerScript instance;

    public GameObject[] dots;
    public List<GameObject[]> connections;
    public GameObject mouseConnection;
    public GameObject chosenDot;
    public GameObject chosenDot2;

    private ParticleSystem connPS1;
    private ParticleSystem connPS2;

    public Color32 incorrectColor;
    public Color32[] colorPalette = new Color32[8];

    [SerializeField]
    private GameObject connectionPrefab;
    [SerializeField]
    private GameObject squareConnectionPrefab;

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

        connPS1 = GameObject.Find("Connection Particle System - Dot 1").GetComponent<ParticleSystem>();
        connPS2 = GameObject.Find("Connection Particle System - Dot 2").GetComponent<ParticleSystem>();

        setColorPalette();
    }

    private void Update()
    {
        GetMouse();

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
                DotScript cd1script = chosenDot.GetComponent<DotScript>();
                DotScript cd2script = chosenDot2.GetComponent<DotScript>();

                GameObject line;

                if (cd1script.shape == "square" || cd2script.shape == "square")
                {
                    line = Instantiate(squareConnectionPrefab);
                }
                else
                {
                    line = Instantiate(connectionPrefab);
                }

                LineRenderer lineRend = line.GetComponent<LineRenderer>();

                lineRend.gameObject.SetActive(true);
                Vector3 sp = chosenDot.transform.position;
                Vector3 ep = chosenDot2.transform.position;
                ep.z = sp.z;
                lineRend.SetPosition(0, sp);
                lineRend.SetPosition(1, ep);

                lineRend.startColor = assignColor(cd1script);
                lineRend.endColor = assignColor(cd2script);

                findNewColliderPositions(chosenDot.transform, chosenDot2.transform, out float posx, out float posy);
                
                if (cd1script.shape == "square")
                {
                    sp.x += posx * 0.4f;
                    sp.y += posy * 0.4f;
                }
                else
                {
                    sp.x += posx * chosenDot.GetComponent<CircleCollider2D>().radius * 0.999f;
                    sp.y += posy * chosenDot.GetComponent<CircleCollider2D>().radius * 0.999f;
                }
                
                if (cd2script.shape == "square")
                {
                    ep.x -= posx * 0.4f;
                    ep.y -= posy * 0.4f;
                }
                else
                {
                    ep.x -= posx * chosenDot2.GetComponent<CircleCollider2D>().radius * 0.999f;
                    ep.y -= posy * chosenDot2.GetComponent<CircleCollider2D>().radius * 0.999f;
                }

                PolygonCollider2D collider = lineRend.GetComponent<PolygonCollider2D>();
                collider.points = new[] { new Vector2(sp.x + 0.01f, sp.y + 0.01f), new Vector2(ep.x + 0.01f, ep.y + 0.01f),
                    new Vector2(ep.x - 0.01f, ep.y - 0.01f), new Vector2(sp.x - 0.01f, sp.y - 0.01f) };

                GameObject[] info = new GameObject[3];
                info[0] = lineRend.gameObject;
                info[1] = chosenDot;
                info[2] = chosenDot2;
                connections.Add(info);

                setOffParticleSystems(chosenDot, chosenDot2);

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
                if (chosenDot2 != null)
                {
                    DotScript cd1script = chosenDot.GetComponent<DotScript>();
                    DotScript cd2script = chosenDot2.GetComponent<DotScript>();

                    compatibleConnection = checkDotMetadata(cd1script, cd2script) && checkDotMetadata(cd2script, cd1script) && plausibleLine;
                }

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
                else if (compatibleConnection) { line.startColor = Color.white; line.endColor = Color.white; }
                else { line.startColor = incorrectColor; line.endColor = incorrectColor; }

                collider.points = new[] { new Vector2(sp.x + 0.01f, sp.y + 0.01f), new Vector2(ep.x + 0.01f, ep.y + 0.01f),
                    new Vector2(ep.x - 0.01f, ep.y - 0.01f), new Vector2(sp.x - 0.01f, sp.y - 0.01f) };
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

    private void findNewColliderPositions(Transform trans1, Transform trans2, out float posx, out float posy)
    {
        float angle = Mathf.Atan2(trans2.position.y - trans1.position.y, trans2.position.x - trans1.position.x);

        posx = Mathf.Cos(angle);
        posy = Mathf.Sin(angle);

        if (posx < 0.001 && posx > -0.001) posx = 0;
        if (posy < 0.001 && posy > -0.001) posy = 0;
    }

    private void setOffParticleSystems(GameObject pos1, GameObject pos2)
    {
        connPS1.transform.position = pos1.transform.position;
        connPS2.transform.position = pos2.transform.position;

        connPS1.startColor = assignColor(pos1.GetComponent<DotScript>());
        connPS2.startColor = assignColor(pos2.GetComponent<DotScript>());

        if (connPS1.isPlaying) connPS1.Stop();
        if (connPS2.isPlaying) connPS2.Stop();
        connPS1.Play();
        connPS2.Play();
    }

    private Color32 assignColor(DotScript dot)
    {
        if (dot.color == "white") return colorPalette[0];
        else if (dot.color == "red") return colorPalette[1];
        else if (dot.color == "orange") return colorPalette[2];
        else if (dot.color == "yellow") return colorPalette[3];
        else if (dot.color == "green") return colorPalette[4];
        else if (dot.color == "blue") return colorPalette[5];
        else if (dot.color == "purple") return colorPalette[6];
        else return colorPalette[7];
    }

    private void setColorPalette()
    {
        // White
        colorPalette[0] = new Color32(255, 255, 255, 255);
        // Red
        colorPalette[1] = new Color32(198, 0, 0, 255);
        // Orange
        colorPalette[2] = new Color32(255, 121, 48, 255);
        // Yellow
        colorPalette[3] = new Color32(237, 216, 57, 255);
        // Green
        colorPalette[4] = new Color32(73, 170, 16, 255);
        // Blue
        colorPalette[5] = new Color32(97, 162, 255, 255);
        // Purple
        colorPalette[6] = new Color32(162, 113, 255, 255);
        // Black
        colorPalette[7] = new Color32(0, 0, 0, 255);
        // Pink
        incorrectColor = new Color32(255, 0, 255, 255);
    }
}
