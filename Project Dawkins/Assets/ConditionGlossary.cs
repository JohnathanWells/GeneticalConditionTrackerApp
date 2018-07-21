using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionGlossary {

    public static Dictionary<string, ConditionsClass> glossary;

    public static void StartUpGlossary()
    {
        glossary = new Dictionary<string, ConditionsClass>();
    }
	
    public static void AddCondition(ConditionsClass con)
    {
        if (!glossary.ContainsKey(con.name))
        {
            glossary.Add(con.name, con);
        }
    }

    public static ConditionsClass GetCondition(string named)
    {
        if (glossary.ContainsKey(named))
        {
            return glossary[named];
        }
        else
        {
            AddCondition(new ConditionsClass(named));
            return glossary[named];
        }
    }
}
