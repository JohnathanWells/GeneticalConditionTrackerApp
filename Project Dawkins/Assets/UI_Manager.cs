using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public float minZoom = 0.1f;
    public float maxZoom = 1.5f;
    public float defaultZoom = 1f;
    public float zoomIncrements = 0.1f;
    public Transform contentHolder;
    public float currentZoom;
    public static bool editingInformation;

	// Use this for initialization
	void Start () {
        currentZoom = defaultZoom;
	}

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            InputZoom(Mathf.Sign(Input.mouseScrollDelta.y));
        }
    }

    public float ChangeZoomInput(float byUnits)
    {
        currentZoom = Mathf.Clamp(currentZoom + (byUnits * zoomIncrements), minZoom, maxZoom);
        return currentZoom;
    }

    public void InputZoom(float x)
    {
        ZoomControl(ChangeZoomInput(x));
    }

    public void ZoomControl(float to)
    {
        contentHolder.localScale = (Vector3.right + Vector3.up) * currentZoom + Vector3.forward;
    }
}
