using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionInstanceClass{

    public ConditionsClass condition;
    public string state;
    public int manifestedAtAge;

    public ConditionInstanceClass()
    {
        condition = null;
        state = "";
        manifestedAtAge = 0;
    }

    public ConditionInstanceClass(ConditionsClass con, string st, int ag)
    {
        condition = con;
        state = st;
        manifestedAtAge = ag;
    }
}
