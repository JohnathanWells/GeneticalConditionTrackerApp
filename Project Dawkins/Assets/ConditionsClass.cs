using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionsClass{
    public string name;

    public ConditionsClass()
    {
        name = "";
    }

    public ConditionsClass(string nm)
    {
        name = nm;
    }
}
