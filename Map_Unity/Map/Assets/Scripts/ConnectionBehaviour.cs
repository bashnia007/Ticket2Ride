using System;
using UnityEngine;
using System.Collections;

public class ConnectionBehaviour : MonoBehaviour {
    private Color hoverColor = Color.white;
    public Color VisibleColor;

    private Renderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        //renderer.material.color = VisibleColor;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material.color = VisibleColor;
        }
    }
}
