using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemberWindowScript : MonoBehaviour {

    public Image windowSprite;
    public Text idField;
    public Text nameField;
    public Text DoBField;
    public Text DoDField;
    //public FamilyMemberClass assignedMember;
    public int assignedMemberID;
    public RectTransform attachedWindow;
    public NailScript motherNail;
    public NailScript fatherNail;
    public NailScript offspringNail;

    void Awake()
    {
        attachedWindow = this.GetComponent<RectTransform>();
    }

    //public void SetPerson(FamilyMemberClass to)
    //{
    //    assignedMember = to;
        
    //}

    public void SetPerson(int to)
    {
        assignedMemberID = to;
    }

    public void UpdatePersonInformation()
    {
        FamilyMemberClass assignedMember = FamilyManager.instance.GetMemberByID(assignedMemberID);
       
       // Debug.Log("Updating");
        if (assignedMember != null)
        {
            //Debug.Log("Pass");
            if (assignedMember.sex == Gender.Male)
            {
                windowSprite.color = ConstantColors.maleWindowsColor;
            }
            else
            {
                windowSprite.color = ConstantColors.femaleWindowsColor;
            }

            if (assignedMember.middleName.Length > 0)
                nameField.text = assignedMember.lastName + ", " + assignedMember.firstName + " " + assignedMember.middleName[0];
            else
                nameField.text = assignedMember.lastName + ", " + assignedMember.firstName;

            DoBField.text = "DoB: " + assignedMember.dayOfBirth.month + "/" + assignedMember.dayOfBirth.day + "/" + assignedMember.dayOfBirth.year;

            if (assignedMember.dayOfDeath.month < 0)
                DoDField.text = "Alive";
            else
                DoDField.text = "DoB: " + assignedMember.dayOfDeath.month + "/" + assignedMember.dayOfDeath.day + "/" + assignedMember.dayOfDeath.year;

            idField.text = "#" + assignedMember.ID;
        }
    }

    public void CreateRelative(string relationship)
    {
        if (relationship == "father")
        {
            FamilyManager.instance.OpenFormulary();
        }
        else if (relationship == "mother")
        {
            FamilyManager.instance.OpenFormulary();
        }
        else if (relationship == "offspring")
        {
            FamilyManager.instance.OpenFormulary();
        }
    }

    public void EditMember()
    {
        FamilyManager.instance.OpenFormulary(assignedMemberID);
    }

    public void SetMother(FamilyMemberClass to)
    {
        FamilyManager.instance.SetMotherToID(assignedMemberID, to.ID);
        //assignedMember.motherID = to.ID;
    }

    public void SetFather(FamilyMemberClass to)
    {
        FamilyManager.instance.SetFatherToID(assignedMemberID, to.ID);
        //assignedMember.fatherID = to.ID;
    }

    public void AddOffspring(FamilyMemberClass to)
    {
        FamilyManager.instance.AddOffspringToID(assignedMemberID, to.ID);
        //assignedMember.AddOffspring(to);
    }

    public void RemoveMother()
    {
        FamilyManager.instance.RemoveMotherFromID(assignedMemberID);
        //assignedMember.motherID = -1;
    }

    public void RemoveFather()
    {
        FamilyManager.instance.RemoveFatherFromID(assignedMemberID);
        //assignedMember.fatherID = -1;
    }

    public void RemoveOffspring(FamilyMemberClass withID)
    {
        FamilyManager.instance.RemoveOffspringFromID(assignedMemberID, withID.ID);
        //assignedMember.RemoveOffspring(withID);
    }

    public void DestroyObject()
    {
        motherNail.DeleteAllTies();
        fatherNail.DeleteAllTies();
        offspringNail.DeleteAllTies();
        FamilyManager.instance.DeleteMember(assignedMemberID);
        Destroy(this.gameObject);
    }
}
