using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NailScript : MonoBehaviour, IPointerDownHandler{

    public MemberWindowScript parentWindow;
    public enum Role { Mother, Father, Offspring}
    public Role thisNailRole;
    List<LineScript> tiedLine = new List<LineScript>();
    LineScript.Extremes extremeOfLine = LineScript.Extremes.None;

    public void OnPointerDown(PointerEventData args)
    {
        //PinInteractionManager.instance.PinInteraction(this);

        if (!UI_Manager.editingInformation)
        {
            //if (args.button == PointerEventData.InputButton.Left || PinInteractionManager.instance.currentInteraction == PinInteractionManager.interactionMode.Tying)
            //    PinInteractionManager.instance.PinInteraction(this);
            //else if (args.button == PointerEventData.InputButton.Right || PinInteractionManager.instance.currentInteraction == PinInteractionManager.interactionMode.Cutting)
            //    DeleteAllTies();

            if (PinInteractionManager.instance.currentInteraction == PinInteractionManager.interactionMode.Tying)
                PinInteractionManager.instance.PinInteraction(this);
            else if (PinInteractionManager.instance.currentInteraction == PinInteractionManager.interactionMode.Cutting)
                DeleteAllTies();
        }
    }

    public void SetTiedLine(LineScript to, NailScript otherExtreme)
    {

        if (otherExtreme != null)
        {
            UpdateConnection(to, otherExtreme);
        }
        //else
        //{
        //    switch(thisNailRole)
        //    {
        //        case Role.Father:
        //            parentWindow.assignedMember.fatherID = -1;
        //            break;
        //        case Role.Mother:
        //            parentWindow.assignedMember.motherID = -1;
        //            break;
        //    }
        //}
    }

    public void untieLine()
    {
        foreach (LineScript l in tiedLine)
            l.UntiePin(this);

    }

    public void UpdateConnection(LineScript to, NailScript otherExtreme)
    {
        switch (thisNailRole)
        {
            case Role.Father:
                {
                    if (tiedLine.Count > 0)
                    {
                        for (int n = tiedLine.Count - 1; n >= 0; n--)
                            tiedLine[n].DeleteLine();

                        tiedLine.Clear();
                    }

                    tiedLine.Add(to);

                    if (otherExtreme != null)
                        FamilyManager.instance.SetFatherToID(parentWindow.assignedMemberID, otherExtreme.parentWindow.assignedMemberID);

                    break;
                }
            case Role.Mother:
                {
                    if (tiedLine.Count > 0)
                    {
                        for (int n = tiedLine.Count - 1; n >= 0; n--)
                            tiedLine[n].DeleteLine();

                        tiedLine.Clear();
                    }

                    tiedLine.Add(to);                    

                    if (otherExtreme != null)
                        FamilyManager.instance.SetMotherToID(parentWindow.assignedMemberID, otherExtreme.parentWindow.assignedMemberID);

                    break;
                }
            case Role.Offspring:
                {
                    if (tiedLine.Find(x => x.Equals(to)) == null)
                    {
                        tiedLine.Add(to);

                        //Debug.Log("adding child");
                        if (otherExtreme != null)
                        {
                            FamilyManager.instance.AddOffspringToID(parentWindow.assignedMemberID, otherExtreme.parentWindow.assignedMemberID);
                        }

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
        }
    }

    public bool CompatibleWithOtherExtreme(NailScript otherExtreme)
    {
        if (otherExtreme.thisNailRole != thisNailRole && otherExtreme.parentWindow.assignedMemberID != parentWindow.assignedMemberID)
        {
            //Debug.Log(otherExtreme.parentWindow.assignedMember.sex.ToString());
            if (thisNailRole == Role.Offspring && otherExtreme.CompatibleWithOtherExtreme(this))
                return true;
            if (thisNailRole == Role.Father && FamilyManager.instance.GetMemberByID(otherExtreme.parentWindow.assignedMemberID).sex == Gender.Male)
                return true;
            else if (thisNailRole == Role.Mother && FamilyManager.instance.GetMemberByID(otherExtreme.parentWindow.assignedMemberID).sex == Gender.Female)
                return true;
            else
                return false;
        }
        else
        {
            Debug.Log("That doesn't make any sense - " + otherExtreme.thisNailRole.ToString() + " _ " + thisNailRole.ToString());
            return false;
        }
    }

    public void DeleteAllTies()
    {
        //Debug.Log("Deleting");
        for (int n = tiedLine.Count - 1; n >= 0; n--)
        {
            DeleteTie(tiedLine[n]);
        }

        tiedLine.Clear();
    }

    //public bool DeleteTie(NailScript connectedTo)
    //{
    //    LineScript temp = null;

    //    if (extremeOfLine == LineScript.Extremes.B)
    //        temp = tiedLine.Find(x => x.pointA == connectedTo);
    //    else if (extremeOfLine == LineScript.Extremes.A)
    //        temp = tiedLine.Find(x => x.pointB == connectedTo);

    //    if (temp != null)
    //    {
    //        if (thisNailRole == Role.Father)
    //        {
    //            FamilyManager.instance.RemoveOffspringFromID(connectedTo.parentWindow.assignedMemberID, this.parentWindow.assignedMemberID);
    //        }
    //        else if (thisNailRole == Role.Mother)
    //        {
    //            FamilyManager.instance.RemoveOffspringFromID(connectedTo.parentWindow.assignedMemberID, this.parentWindow.assignedMemberID);
    //        }
    //        else if (thisNailRole == Role.Offspring)
    //        {
    //            if (this.parentWindow.assignedMember.sex == Gender.Female)
    //            {
    //                connectedTo.parentWindow.assignedMember.motherID = -1;
    //            }
    //            else
    //            {
    //                connectedTo.parentWindow.assignedMember.fatherID = -1;
    //            }
    //        }

    //        if (tiedLine.Contains(temp))
    //            tiedLine.Remove(temp);
    //        temp.DeleteLine();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    public void DeleteConnection(int toID, LineScript andLine)
    {
        tiedLine.Remove(andLine);

        if (thisNailRole == Role.Father)
        {
            FamilyManager.instance.RemoveFatherFromID(parentWindow.assignedMemberID);
        }
        else if (thisNailRole == Role.Mother)
        {
            FamilyManager.instance.RemoveMotherFromID(parentWindow.assignedMemberID);
        }
        else if (thisNailRole == Role.Offspring)
        {
            FamilyManager.instance.RemoveOffspringFromID(parentWindow.assignedMemberID, toID);
        }
    }

    public bool DeleteTie(LineScript temp)
    {
        //NailScript connectedTo = null;

        //if (extremeOfLine == LineScript.Extremes.B)
        //    connectedTo = temp.pointA;
        //else if (extremeOfLine == LineScript.Extremes.A)
        //    connectedTo = temp.pointB;

        if (temp != null)
        {
            //tiedLine.Remove(temp);
            temp.DeleteLine();
            return true;
        }
        else
        {
            return false;
        }
    }
}
