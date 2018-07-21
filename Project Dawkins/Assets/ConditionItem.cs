using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionItem : MonoBehaviour {

    public Text conditionName;
    public Text recordsOfIt;
    public string messageForRecordCount;
    public Transform subTagPrefab;
    Transform subTagMenu;
    RecordsOfCondition assignedRecord;
    List<SubTagScript> specificInstances = new List<SubTagScript>();
	

    public void SetParentOfSubItems(Transform to)
    {
        subTagMenu = to;
    }

    public void SetConditionRecord(RecordsOfCondition to)
    {
        assignedRecord = to;
        conditionName.text = to.condition.name;
        recordsOfIt.text = messageForRecordCount.Replace("#", "<b>" + to.membersWithCondition.Count.ToString() + "</b>");

        ShowInstances();
    }

    public void ShowInstances()
    {
        for (int n = specificInstances.Count - 1; n >= 0; n--)
        {
            Destroy(specificInstances[n].gameObject);
            specificInstances.RemoveAt(n);
        }

        SubTagScript tempScript;
        Transform tempTrans;
        FamilyMemberClass tempFamMem;

        foreach (int i in assignedRecord.membersWithCondition)
        {
            tempFamMem = FamilyManager.instance.GetMemberByID(i);
            tempTrans = Instantiate(subTagPrefab, Vector3.zero, Quaternion.identity, subTagMenu);
            tempScript = tempTrans.GetComponent<SubTagScript>();

            if (tempFamMem.middleName.Length > 0)
                tempScript.SetText(tempFamMem.firstName + " " + tempFamMem.middleName[0] + " " + tempFamMem.lastName, tempFamMem.GetCondition(assignedRecord.condition.name));
            else
                tempScript.SetText(tempFamMem.firstName + " " + tempFamMem.lastName, tempFamMem.GetCondition(assignedRecord.condition.name));

            specificInstances.Add(tempScript);
        }
    }
}
