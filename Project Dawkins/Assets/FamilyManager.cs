using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyManager : MonoBehaviour {

    public static FamilyManager instance;
    public Transform windowPrefab;
    public FormularyScript formulary;
    public Color colorForMale;
    public Color colorForFemale;
    public Transform nodeLayer;
    public PinInteractionManager pinManager;
    public float timeBetweenAutosaves = 60f;
    float timeC = 0;
    int selectedMember;
    relationship typeOfRelativeSelected;

    Dictionary<int, FamilyMemberClass> wholeFamily = new Dictionary<int, FamilyMemberClass>();
    Dictionary<int, MemberWindowScript> correspondingWindowsForFamily = new Dictionary<int, MemberWindowScript>();

    void Awake()
    {
        instance = this;
        ConditionGlossary.StartUpGlossary();
        ConstantColors.maleWindowsColor = colorForMale;
        ConstantColors.femaleWindowsColor = colorForFemale;

        LoadFamily();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            DebugFamily();
        }

        timeC += Time.deltaTime;

        if (timeC > timeBetweenAutosaves)
        {
            SaveFamily();
            timeC = 0;
        }

    }

    public void OpenFormulary()
    {
        formulary.OpenFormulary(wholeFamily.Count);
    }

    public void OpenFormulary(int idToOpen)
    {
        formulary.OpenFormulary(idToOpen);
    }

    public MemberWindowScript CreateNewMemberWindow(FamilyMemberClass fromMember, Vector3 atPosition)
    {
        Transform t = Instantiate(windowPrefab, atPosition, Quaternion.identity, nodeLayer);
        t.GetComponent<RectTransform>().anchoredPosition = atPosition;
        MemberWindowScript tempWin;

        if ((tempWin = t.GetComponent<MemberWindowScript>()) != null)
        {

            if (!wholeFamily.ContainsKey(fromMember.ID))
                wholeFamily.Add(fromMember.ID, fromMember);


            tempWin.SetPerson(fromMember.ID);
            tempWin.UpdatePersonInformation();

            if (!correspondingWindowsForFamily.ContainsKey(fromMember.ID))
                correspondingWindowsForFamily.Add(fromMember.ID, tempWin);

            return tempWin;
        }

        return null;
    }

    public MemberWindowScript CreateNewMemberWindow(FamilyMemberClass fromMember)
    {
        Vector3 atPosition = Camera.main.ViewportToWorldPoint(Vector3.one * 0.5f);
        atPosition.z = 0;

        Transform t = Instantiate(windowPrefab, atPosition, Quaternion.identity, nodeLayer);
        t.GetComponent<RectTransform>().anchoredPosition = atPosition;
        MemberWindowScript tempWin;

        if ((tempWin = t.GetComponent<MemberWindowScript>()) != null)
        {

            if (!wholeFamily.ContainsKey(fromMember.ID))
                wholeFamily.Add(fromMember.ID, fromMember);
           

            tempWin.SetPerson(fromMember.ID);
            tempWin.UpdatePersonInformation();

            if (!correspondingWindowsForFamily.ContainsKey(fromMember.ID))
                correspondingWindowsForFamily.Add(fromMember.ID, tempWin);

            return tempWin;
        }

        return null;
    }

    public void DeleteMember(int withID)
    {
        Debug.Log("Deleting member...");
        if (correspondingWindowsForFamily.ContainsKey(withID) && wholeFamily.ContainsKey(correspondingWindowsForFamily[withID].assignedMemberID))
        {
            FamilyMemberClass temp = wholeFamily[correspondingWindowsForFamily[withID].assignedMemberID];

            if (correspondingWindowsForFamily.ContainsKey(temp.motherID))
            {
                RemoveOffspringFromID(temp.motherID, temp.ID);
            }

            if (correspondingWindowsForFamily.ContainsKey(temp.fatherID))
            {
                RemoveOffspringFromID(temp.fatherID, temp.ID);
            }

            if (temp.sex == Gender.Female)
            {
                foreach (int i in temp.offspring)
                {
                    RemoveMotherFromID(i);
                }
            }
            else
            {
                foreach (int i in temp.offspring)
                {
                    RemoveMotherFromID(i);
                }
            }

            correspondingWindowsForFamily.Remove(withID);
            wholeFamily.Remove(withID);
        }

        SaveFamily();
    }

    public void DestroyWindow(int withID)
    {
        //string log = "Looking for " + withID + "\n";

        //foreach (KeyValuePair<int, FamilyMemberClass> i in wholeFamily)
        //{
        //    log += i.Key + " : " + i.Value.firstName + " " + i.Value.lastName + "\n";
        //}

        //Debug.Log(log);

        correspondingWindowsForFamily[withID].DestroyObject();

        //foreach (KeyValuePair<int, FamilyMemberClass> i in wholeFamily)
        //{
        //    log += i.Key + " : " + i.Value.firstName + " " + i.Value.lastName + "\n";
        //}

        //Debug.Log(log);
    }

    public bool UpdateMember(FamilyMemberClass to)
    {
        if (wholeFamily.ContainsKey(to.ID))
        {
            wholeFamily[to.ID] = to;

            if (correspondingWindowsForFamily.ContainsKey(to.ID))
            {
                correspondingWindowsForFamily[to.ID].SetPerson(to.ID);
                correspondingWindowsForFamily[to.ID].UpdatePersonInformation();
            }
            else
            {
                CreateNewMemberWindow(wholeFamily[to.ID]);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public FamilyMemberClass GetMemberByID(int id)
    {
        if (wholeFamily.ContainsKey(id))
        {
            return wholeFamily[id];
        }
        else
            return null;
    }

    public void AddOffspringToID(int id, int to)
    {
        if (wholeFamily.ContainsKey(id) && wholeFamily.ContainsKey(to))
        {
            wholeFamily[id].AddOffspring(wholeFamily[to]);
        }
    }

    public void RemoveOffspringFromID(int id, int removedMember)
    {
        if (wholeFamily.ContainsKey(id))
        {
            wholeFamily[id].RemoveOffspring(removedMember);
        }
    }

    public void SetMotherToID(int id, int to)
    {
        if (wholeFamily.ContainsKey(id) && wholeFamily.ContainsKey(to))
        {
            wholeFamily[id].motherID = to;
        }
    }

    public void SetFatherToID(int id, int to)
    {
        if (wholeFamily.ContainsKey(id) && wholeFamily.ContainsKey(to))
        {
            wholeFamily[id].fatherID = to;
        }
    }

    public void RemoveMotherFromID(int id)
    {
        if (wholeFamily.ContainsKey(id))
        {
            wholeFamily[id].motherID = -1;
        }
    }

    public void RemoveFatherFromID(int id)
    {
        if (wholeFamily.ContainsKey(id))
        {
            wholeFamily[id].fatherID = -1;
        }
    }

    public void SaveFamily()
    {
        List<FamilyMemberClass> currentFamily = new List<FamilyMemberClass>();

        foreach (KeyValuePair<int, MemberWindowScript> w in correspondingWindowsForFamily)
        {
            Vector3 pos = w.Value.transform.localPosition;
            //Debug.Log(pos);

            wholeFamily[w.Key].windowPosition = new float[]{ pos.x, pos.y, 0};
            currentFamily.Add(wholeFamily[w.Key]);
        }

        SaveLoad.Save(currentFamily);
    }

    public void LoadFamily()
    {
        if (!SaveLoad.CheckIfPathIsEmpty())
        {
            List<FamilyMemberClass> getList;
            SaveLoad.Load(out getList);
            Vector3 pos;
            //Debug.Log(getList.Count);

            foreach (FamilyMemberClass f in getList)
            {
                pos = new Vector3(f.windowPosition[0], f.windowPosition[1], f.windowPosition[2]);
                CreateNewMemberWindow(f, pos);
            }

            ConnectWindows();

        }
        else
        {
            Debug.Log("File not found!");
        }
    }

    public void ConnectWindows()
    {
        NailScript[] temp = this.gameObject.GetComponentsInChildren<NailScript>();
        pinManager.currentInteraction = PinInteractionManager.interactionMode.Tying;

        foreach (NailScript l in temp)
        {
            l.DeleteAllTies();
        }

        MemberWindowScript tempWindow;
        FamilyMemberClass tempMember;

        foreach (KeyValuePair<int, MemberWindowScript> k in correspondingWindowsForFamily)
        {
            tempWindow = k.Value;

            if (wholeFamily.ContainsKey(tempWindow.assignedMemberID))
            {
                tempMember = wholeFamily[tempWindow.assignedMemberID];

                if (correspondingWindowsForFamily.ContainsKey(tempMember.fatherID))
                {
                    //Debug.Log(correspondingWindowsForFamily.ContainsKey(tempMember.fatherID) + " " + tempMember.fatherID);
                    pinManager.PinInteraction(tempWindow.fatherNail);
                    //Debug.Log(correspondingWindowsForFamily.ContainsKey(tempMember.fatherID) + " " + tempMember.fatherID);
                    pinManager.PinInteraction(correspondingWindowsForFamily[tempMember.fatherID].offspringNail);
                }

                if (correspondingWindowsForFamily.ContainsKey(tempMember.motherID))
                {
                    pinManager.PinInteraction(tempWindow.motherNail);
                    pinManager.PinInteraction(correspondingWindowsForFamily[tempMember.motherID].offspringNail);
                }


                if (tempMember.sex == Gender.Male)
                {
                    for (int i = 0; i < tempMember.offspring.Count; i++ )
                    {
                        if (correspondingWindowsForFamily.ContainsKey(tempMember.offspring[i]))
                        {
                            pinManager.PinInteraction(tempWindow.offspringNail);
                            pinManager.PinInteraction(correspondingWindowsForFamily[tempMember.offspring[i]].fatherNail);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tempMember.offspring.Count; i++)
                    {
                        if (correspondingWindowsForFamily.ContainsKey(tempMember.offspring[i]))
                        {
                            pinManager.PinInteraction(tempWindow.offspringNail);
                            pinManager.PinInteraction(correspondingWindowsForFamily[tempMember.offspring[i]].motherNail);
                        }
                    }
                }
                //DebugFamily();
            }
            else
                Debug.Log("Failed to find " + tempWindow.assignedMemberID);
        }
    }

    public void DebugFamily()
    {
        foreach (KeyValuePair<int, FamilyMemberClass> fromMember in wholeFamily)
        {
            string str = "Spawned #" + fromMember.Key + ": " + fromMember.Value.firstName + " " + fromMember.Value.middleName + " " + fromMember.Value.lastName + "\nMother: " + fromMember.Value.motherID + "\nFather: " + fromMember.Value.fatherID + "\nChildren: ";
            foreach (int i in fromMember.Value.offspring)
                str += i + ",";
            Debug.Log(str);
        }
    }
}
