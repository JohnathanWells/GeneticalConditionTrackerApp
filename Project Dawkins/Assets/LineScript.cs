using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineScript : MonoBehaviour, IPointerDownHandler {

    public NailScript pointA;
    public NailScript pointB;
    public LineRenderer rend;
    bool readyForTying = false;

    public enum Extremes { None, A, B };

    void Update()
    {
        //if (pointA == null && pointB == null)
        //    DeleteLine();
        //else
        //{

            if (pointA != null)
                rend.SetPosition(0, pointA.transform.position);
            else
            {
                if (PinInteractionManager.instance.currentLine.Equals(this))
                    rend.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                else if (readyForTying)
                    DeleteLine();
            }

            if (pointB != null)
                rend.SetPosition(1, pointB.transform.position);
            else
            {
                if (PinInteractionManager.instance.currentLine.Equals(this))
                    rend.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                else if (readyForTying)
                    DeleteLine();
            }
        //}
    }

    public void SetReady()
    {
        readyForTying = true;
    }

    public void SetA(NailScript to)
    {
        pointA = to;

        to.SetTiedLine(this, pointB);

        if (pointB != null)
            pointB.UpdateConnection(this, pointA);
    }

    public void SetB(NailScript to)
    {
        pointB = to;
        to.SetTiedLine(this, pointA);

        if (pointA != null)
            pointA.UpdateConnection(this, pointB);
    }

    public void UnpinA()
    {
        pointA = null;
    }

    public void UnpinB()
    {
        pointB = null;
    }

    public void UntiePin(NailScript x)
    {
        int a=-1, b=-1;
        if (pointA != null)
             a = pointA.parentWindow.assignedMemberID;
        if (pointB != null)
            b = pointB.parentWindow.assignedMemberID;

        if (pointA.Equals(x))
        {
            //pointA = null;
            //x.SetTiedLine(null, Extremes.None);
            pointA.DeleteConnection(b, this);
        }
        else if (pointB.Equals(x))
        {
            //pointB = null;
            //x.SetTiedLine(null, Extremes.None);
            pointB.DeleteConnection(a, this);
        }
    }

    public void DeleteLine()
    {
        if (pointA != null)
            //pointA.SetTiedLine(null, Extremes.None);
            UntiePin(pointA);

        if (pointB != null)
            //pointB.SetTiedLine(null, Extremes.None);
            UntiePin(pointB);

        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData args)
    {
        Debug.Log("Snap");
    }
}
