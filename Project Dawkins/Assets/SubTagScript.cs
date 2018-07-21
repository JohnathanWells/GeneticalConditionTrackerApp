using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubTagScript : MonoBehaviour {

    public Text nameOfPerson;
    public Text ageAtCondition;
    public Text details;

    public void SetText(string name, ConditionInstanceClass to)
    {
        nameOfPerson.text = name;
        ageAtCondition.text = to.manifestedAtAge.ToString();
        details.text = to.state;
    }
}
