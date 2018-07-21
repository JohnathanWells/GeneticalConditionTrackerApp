using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingScript : MonoBehaviour {

    public RectTransform target;
    public Vector3 positionA;
    public Vector3 positionB;
    public iTween.EaseType typeOfMotion;
    public float timeTakenByAnimation;
    
    public void MoveToPointA()
    {
        target.anchoredPosition = positionA;
        //iTween.MoveFrom(gameObject, iTween.Hash(
        //    "position", positionB,
        //    "easetype", typeOfMotion,
        //    "time", timeTakenByAnimation));
    }

    public void MoveToPointB()
    {
        target.anchoredPosition = positionB;
        //iTween.MoveFrom(gameObject, iTween.Hash(
        //    "position", positionA,
        //    "easetype", typeOfMotion,
        //    "time", timeTakenByAnimation));
    }

    void Move(Vector3 to)
    {
        target.anchoredPosition = to;
    }
}
