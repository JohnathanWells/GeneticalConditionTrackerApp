using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class RecordsOfCondition
{
    public ConditionsClass condition;
    public List<int> membersWithCondition = new List<int>();

    public RecordsOfCondition(ConditionsClass cond, List<int> mem)
    {
        condition = cond;
        membersWithCondition = mem;
    }
}

public class ConditionListingScript : MonoBehaviour {

    public FamilyMemberClass selectedMember;
    public int maximumDepth = 3;
    public List<RecordsOfCondition> cachedRecords;
    public Transform recordPrefab;
    public Transform parentOfObjects;
    public InputField depthInput;
    

    public void SpawnList()
    {
        ClearList();

        depthInput.text = maximumDepth.ToString();

        ConditionItem temp;

        foreach(RecordsOfCondition r in cachedRecords)
        {
            Debug.Log(r.condition.name);
            temp = Instantiate(recordPrefab, Vector3.zero, Quaternion.identity, parentOfObjects).GetComponent<ConditionItem>();
            temp.SetParentOfSubItems(parentOfObjects);
            temp.SetConditionRecord(r);
            //temp.SendMessage("SetConditionRecord", r/*, SendMessageOptions.DontRequireReceiver*/);
            //Debug.Log(temp.name);
        }
    }

    public void ClearList()
    {
        Transform[] temp = parentOfObjects.GetComponentsInChildren<Transform>();

        for (int n = temp.Length - 1; n > 0; n--)
        {
            Destroy(temp[n].gameObject);
        }
    }

    public void SetMaxDepth()
    {
        int to;

        if (int.TryParse(depthInput.text, out to) && to > 0)
        {
            maximumDepth = to;
            SetConditionListing(selectedMember);
        }
    }


    //Functionality
    public void SetConditionListing(FamilyMemberClass to)
    {
        selectedMember = to;
        depthInput.text = maximumDepth.ToString();
        UpdateCache();
        SpawnList();
    }

    public void UpdateCache()
    {
        if (selectedMember != null)
        {
            cachedRecords = GetConditionsWithinRange(selectedMember.ID);
            //SpawnList();
        }
    }

    public List<RecordsOfCondition> GetConditionsWithinRange(int from)
    {
        Dictionary<string, List<int>> tempDict = new Dictionary<string, List<int>>();

        LogConditionsIntoDictionary(from, tempDict, 0, -1);

        List<RecordsOfCondition> result = new List<RecordsOfCondition>();
        
        foreach(KeyValuePair<string, List<int>> k in tempDict)
        {
            result.Add(new RecordsOfCondition(ConditionGlossary.GetCondition(k.Key), k.Value));
        }
        
        return result/*.OrderBy(x=>x.membersWithCondition.Count).ToList()*/;
    }

    public void LogConditionsIntoDictionary(int fromID, Dictionary<string, List<int>> records, int currentDepth, int comingFromID)
    {
        Debug.Log("Currently checking depth for " + fromID);
        FamilyMemberClass temp = FamilyManager.instance.GetMemberByID(fromID);

        foreach(ConditionInstanceClass c in temp.listedConditions)
        {
            if (records.ContainsKey(c.condition.name))
            {
                if (!records[c.condition.name].Contains(fromID))
                {
                    records[c.condition.name].Add(fromID);
                }
            }
            else
            {
                records.Add(c.condition.name, new List<int>(){ fromID});
            }
        }

        if (currentDepth < maximumDepth)
        {
            foreach (int i in temp.offspring)
            {
                if (comingFromID != i && i >= 0)
                    LogConditionsIntoDictionary(i, records, currentDepth + 1, fromID);
            }

            if (comingFromID != temp.motherID && temp.motherID >= 0)
                LogConditionsIntoDictionary(temp.motherID, records, currentDepth + 1, fromID);

            if (comingFromID != temp.fatherID && temp.fatherID >= 0)
                LogConditionsIntoDictionary(temp.fatherID, records, currentDepth + 1, fromID);
        }
    }

}
