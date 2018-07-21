using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableObjectScript : MonoBehaviour, IDragHandler{
        
    public void OnDrag(PointerEventData args)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(args.position);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}
