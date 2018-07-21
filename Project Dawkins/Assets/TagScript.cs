using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TagScript : MonoBehaviour {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI ageText;
    public string tag;
    public ConditionInstanceClass condition;

    public void SetText(ConditionInstanceClass to)
    {
        //condition = ConditionGlossary.GetCondition(to.condition.name);
        condition = to;
        nameText.text = to.condition.name;
        phaseText.text = to.state;
        ageText.text = to.manifestedAtAge.ToString();
        tag = to.condition.name;
    }

    public void DeleteTag()
    {
        FamilyManager.instance.formulary.EliminateCondition(tag);
    }
}
