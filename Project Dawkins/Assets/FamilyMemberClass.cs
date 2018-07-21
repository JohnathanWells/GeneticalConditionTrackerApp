using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender { Male, Female};

[System.Serializable]
public class FamilyMemberClass{
    public int ID;
    public string firstName;
    public string middleName;
    public string lastName;
    public Gender sex;
    public DateClass dayOfBirth;
    public DateClass dayOfDeath;
    public List<ConditionInstanceClass> listedConditions;
    public int motherID;
    public int fatherID;
    public List<int> offspring;
    public float[] windowPosition = new float[3];

    public FamilyMemberClass()
    {
        firstName = "";
        middleName = "";
        lastName = "";
        offspring = new List<int>();
        listedConditions = new List<ConditionInstanceClass>();
    }

    public FamilyMemberClass(string fN, string mN, string lN, Gender s, DateClass dob, DateClass dod)
    {
        firstName = fN;
        middleName = mN;
        lastName = lN;
        sex = s;
        dayOfBirth = dob;
        dayOfDeath = dod;
        motherID = -1;
        motherID = -1;
        offspring = new List<int>();
        listedConditions = new List<ConditionInstanceClass>();
    }

    public void SetConditions(List<ConditionInstanceClass> to)
    {
        listedConditions.Clear();
        foreach(ConditionInstanceClass s in to)
        {
            listedConditions.Add(s);
        }
    }

    public void SetParent(FamilyMemberClass to)
    {
        if (to.sex == Gender.Female)
        {
            motherID = to.ID;
            to.AddOffspring(this);
        }
        else
        {
            fatherID = to.ID;
            to.AddOffspring(this);
        }
    }

    public void AddOffspring(FamilyMemberClass member)
    {
        offspring.Add(member.ID);

        //if (sex == Gender.Female)
        //{
        //    member.motherID = ID;
        //}
        //else
        //{
        //    member.fatherID = ID;
        //}
    }

    public void AddCondition(ConditionInstanceClass x)
    {
        if (!listedConditions.Contains(x))
            listedConditions.Add(x);
    }

    public ConditionInstanceClass GetCondition(string withName)
    {
        return listedConditions.Find(x => x.condition.name == withName);
    }

    public void RemoveCondition(string withName)
    {
        listedConditions.RemoveAll(x => x.condition.name == withName);
    }

    public void RemoveOffspring(FamilyMemberClass member)
    {
        int ind = offspring.FindIndex(x => x == member.ID);
        if (ind >= 0)
        {
            offspring.RemoveAt(ind);
        }
    }

    public void RemoveOffspring(int withID)
    {
        int ind = offspring.FindIndex(x => x == withID);
        if (ind >= 0)
            offspring.RemoveAt(ind);
    }
}
