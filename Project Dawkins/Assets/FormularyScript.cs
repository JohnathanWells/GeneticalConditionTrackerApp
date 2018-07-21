using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum relationship { Father, Mother, Son, Daughter};

public class FormularyScript : MonoBehaviour {
    
    [System.Serializable]
    public class dateField
    {
        public TMP_InputField monthField;
        public TMP_InputField dayField;
        public TMP_InputField yearField;
    }

    //[Header("Function Targets")]
    //public Transform manager;
    public Button informationTabButton;
    public SlidingScript animatorOfFormulary;
    public Image backgroundOfFormulary;
    public Button createButton;
    public Button submitButton;
    Color colorForMale;
    Color colorForFemale;

    [Header("Tag Management")]
    public TMP_InputField conditionEnterer;
    public TMP_InputField conditionAgeEnterer;
    public TMP_InputField conditionDetailEnterer;
    public Transform prefabForTags;
    public Transform parentForTags;

    [Header("Fields")]
    public TMP_InputField firstNameField;
    public TMP_InputField middleNameField;
    public TMP_InputField lastNameField;
    public Dropdown gender;
    public dateField DOBField;
    public dateField DODField;
    public TextMeshProUGUI errorMessage;
    //private relationship relationToMember;
    private FamilyMemberClass selectedMember;
    private int id;
    private Dictionary<string, TagScript> conditionTags = new Dictionary<string, TagScript>();

    [Header("Condition Listing")]
    public ConditionListingScript condLisScript;

    bool conditionsBrowsed = false;

    void Awake()
    {
        colorForMale = ConstantColors.maleWindowsColor;
        colorForFemale = ConstantColors.femaleWindowsColor;
    }

    public void OpenFormulary(int ID)
    {
        if (selectedMember != null && ID != selectedMember.ID)
            conditionsBrowsed = false;

        id = ID;
        ResetFormulary();
        ShowFormulary();
        UpdateColor();
        informationTabButton.onClick.Invoke();
        UI_Manager.editingInformation = true;
        PinInteractionManager.instance.CancelPinning();
    }

    void ResetFormulary()
    {
        selectedMember = FamilyManager.instance.GetMemberByID(id);
        if (selectedMember == null)
        {
            ClearFields();

            createButton.transform.gameObject.SetActive(true);
            submitButton.transform.gameObject.SetActive(false);
        }
        else
        {
            FillFields();

            createButton.transform.gameObject.SetActive(false);
            submitButton.transform.gameObject.SetActive(true);
        }

        ClearError();
    }

    public void CloseFormulary()
    {
        UI_Manager.editingInformation = false;
        HideFormulary();
    }

    public void ShowFormulary()
    {
        animatorOfFormulary.MoveToPointB();
    }

    public void HideFormulary()
    {
        animatorOfFormulary.MoveToPointA();
    }

    public void CreateTempMember()
    {
        if (!string.IsNullOrEmpty(firstNameField.text) && !string.IsNullOrEmpty(lastNameField.text) &&
            !string.IsNullOrEmpty(DOBField.monthField.text) && !string.IsNullOrEmpty(DOBField.dayField.text) && !string.IsNullOrEmpty(DOBField.yearField.text))
        {
            DateClass dob;
            if (DateClass.ConvertStringsToDate(DOBField.monthField.text, DOBField.dayField.text, DOBField.yearField.text, out dob))
            {
                DateClass dod;
                if (!string.IsNullOrEmpty(DODField.monthField.text) || !string.IsNullOrEmpty(DODField.dayField.text) || !string.IsNullOrEmpty(DODField.yearField.text))
                {
                    if (!DateClass.ConvertStringsToDate(DODField.monthField.text, DODField.dayField.text, DODField.yearField.text, out dod))
                        LogError("Only enter numbers in the date fields!");
                    else
                    {
                        FamilyMemberClass newMember = new FamilyMemberClass(firstNameField.text, middleNameField.text, lastNameField.text, (Gender)gender.value, dob, dod);
                        newMember.ID = id;
                        newMember.SetConditions(CreateListFromDictionary());

                        selectedMember = newMember;
                    }
                }
                else
                {
                    dod = new DateClass(-1, -1, -1);

                    FamilyMemberClass newMember = new FamilyMemberClass(firstNameField.text, middleNameField.text, lastNameField.text, (Gender)gender.value, dob, dod);
                    newMember.ID = id;
                    newMember.SetConditions(CreateListFromDictionary());

                    selectedMember = newMember;
                }
            }
            else
            {
                LogError("Only enter numbers in the date fields!");
            }
        }
        else
        {
            LogError("Please fill all fields.");
        }
    }

    public void CreateNewMember()
    {
        if (!string.IsNullOrEmpty(firstNameField.text) && !string.IsNullOrEmpty(lastNameField.text) && 
            !string.IsNullOrEmpty(DOBField.monthField.text) && !string.IsNullOrEmpty(DOBField.dayField.text) && !string.IsNullOrEmpty(DOBField.yearField.text))
        {
            DateClass dob;
            if (DateClass.ConvertStringsToDate(DOBField.monthField.text, DOBField.dayField.text, DOBField.yearField.text, out dob))
            {
                DateClass dod;
                if (!string.IsNullOrEmpty(DODField.monthField.text) || !string.IsNullOrEmpty(DODField.dayField.text) || !string.IsNullOrEmpty(DODField.yearField.text))
                {
                    if (!DateClass.ConvertStringsToDate(DODField.monthField.text, DODField.dayField.text, DODField.yearField.text, out dod))
                        LogError("Only enter numbers in the date fields!");
                    else
                    {
                        FamilyMemberClass newMember = new FamilyMemberClass(firstNameField.text, middleNameField.text, lastNameField.text, (Gender)gender.value, dob, dod);
                        newMember.ID = id;
                        newMember.SetConditions(CreateListFromDictionary());

                        //if (relationToMember == relationship.Father || relationToMember == relationship.Mother)
                        //{
                        //    newMember.AddOffspring(relatedMember);
                        //}
                        //else
                        //{
                        //    newMember.SetParent(relatedMember);
                        //}

                        FamilyManager.instance.CreateNewMemberWindow(newMember);

                        CloseFormulary();
                    }
                }
                else
                {
                    dod = new DateClass(-1, -1, -1);

                    FamilyMemberClass newMember = new FamilyMemberClass(firstNameField.text, middleNameField.text, lastNameField.text, (Gender)gender.value, dob, dod);
                    newMember.ID = id;
                    newMember.SetConditions(CreateListFromDictionary());

                    FamilyManager.instance.CreateNewMemberWindow(newMember);

                    CloseFormulary();
                }
            }
            else
            {
                LogError("Only enter numbers in the date fields!");
            }
        }
        else
        {
            LogError("Please fill all fields.");
        }
    }

    public void SubmitChangesToMember()
    {
        if (!string.IsNullOrEmpty(firstNameField.text) && !string.IsNullOrEmpty(lastNameField.text) &&
            !string.IsNullOrEmpty(DOBField.monthField.text) && !string.IsNullOrEmpty(DOBField.dayField.text) && !string.IsNullOrEmpty(DOBField.yearField.text))
        {
            DateClass dob;
            if (DateClass.ConvertStringsToDate(DOBField.monthField.text, DOBField.dayField.text, DOBField.yearField.text, out dob))
            {
                DateClass dod;
                if (!string.IsNullOrEmpty(DODField.monthField.text) || !string.IsNullOrEmpty(DODField.dayField.text) || !string.IsNullOrEmpty(DODField.yearField.text))
                {
                    if (!DateClass.ConvertStringsToDate(DODField.monthField.text, DODField.dayField.text, DODField.yearField.text, out dod))
                        LogError("Only enter numbers in the date fields!");
                    else
                    {
                        if (selectedMember != null)
                        {
                            LoadInfoIntoMember();
                            FamilyManager.instance.UpdateMember(selectedMember);

                            CloseFormulary();
                        }
                    }
                }
                else
                {
                    dod = new DateClass(-1, -1, -1);

                    if (selectedMember != null)
                    {
                        LoadInfoIntoMember();
                        FamilyManager.instance.UpdateMember(selectedMember);
                    }

                    CloseFormulary();
                }
            }
            else
            {
                LogError("Only enter numbers in the date fields!");
            }
        }
        else
        {
            LogError("Please fill all fields.");
        }
    }

    public void UpdateColor()
    {
        if (gender.value == (int)Gender.Male)
        {
            backgroundOfFormulary.color = ConstantColors.maleWindowsColor;
        }
        else
        {
            backgroundOfFormulary.color = ConstantColors.femaleWindowsColor;
        }
    }

    public void LoadInfoIntoMember()
    {
        if (selectedMember != null)
        {
            selectedMember.firstName = firstNameField.text;
            selectedMember.middleName = middleNameField.text;
            selectedMember.lastName = lastNameField.text;
            selectedMember.sex = (Gender)gender.value;
            selectedMember.SetConditions(CreateListFromDictionary());

            int a, b, c;

            if (int.TryParse(DOBField.monthField.text, out a))
                a = 0;
            if (!int.TryParse(DOBField.dayField.text, out b))
                b = 0;
            if (int.TryParse(DOBField.yearField.text, out c))
            {
                selectedMember.dayOfBirth = new DateClass(a, b, c);
            }

            if (!int.TryParse(DODField.monthField.text, out a))
                a = -1;
            if (!int.TryParse(DODField.dayField.text, out b))
                b = -1;
            if (!int.TryParse(DODField.yearField.text, out c))
                c = 0;

            selectedMember.dayOfDeath = new DateClass(a, b, c);
        }
    }

    public void CrawlForPossibleConditions()
    {
        if (selectedMember != null && !conditionsBrowsed)
        {
            condLisScript.SetConditionListing(selectedMember);
            conditionsBrowsed = true;
        }
    }

    public void LogError (string text)
    {
        errorMessage.text = text;
    }

    public void ClearError()
    {
        errorMessage.text = "";
    }

    public void ClearFields()
    {
        firstNameField.text = "";
        middleNameField.text = "";
        lastNameField.text = "";
        DOBField.monthField.text = "";
        DOBField.dayField.text = "";
        DOBField.yearField.text = "";
        DODField.monthField.text = "";
        DODField.dayField.text = "";
        DODField.yearField.text = "";
        gender.value = 0;

        //foreach (KeyValuePair<string, Transform> k in conditionTags)
        //{
        //    Destroy(k.Value.gameObject);
        //}
        ClearTagBox();
        conditionTags.Clear();
    }

    public void FillFields()
    {
        firstNameField.text = selectedMember.firstName;
        middleNameField.text = selectedMember.middleName;
        lastNameField.text = selectedMember.lastName;
        DOBField.monthField.text = selectedMember.dayOfBirth.month.ToString();
        DOBField.dayField.text = selectedMember.dayOfBirth.day.ToString();
        DOBField.yearField.text = selectedMember.dayOfBirth.year.ToString();

        if (selectedMember.dayOfDeath.month >= 0 && selectedMember.dayOfDeath.month >= 0)
        {
            DODField.monthField.text = selectedMember.dayOfDeath.month.ToString();
            DODField.dayField.text = selectedMember.dayOfDeath.day.ToString();
            DODField.yearField.text = selectedMember.dayOfDeath.year.ToString();
        }

        gender.value = (int)selectedMember.sex;

        //foreach (KeyValuePair<string, Transform> k in conditionTags)
        //{
        //    Destroy(k.Value.gameObject);
        //}
        FillDictionaryFromList(selectedMember.listedConditions);
        UpdateTagBox();
    }

    public void UpdateTagBox()
    {
        ClearTagBox();
        List<ConditionInstanceClass> conditionsInDictionary = selectedMember.listedConditions;

        //foreach (KeyValuePair<string, TagScript> k in conditionTags)
        //{
        //    conditionsInDictionary.Add(k.Value.condition);
        //}

        for (int n = 0; n < conditionsInDictionary.Count; n++)
        {
            ConditionInstanceClass s = conditionsInDictionary[n];
            conditionTags[s.condition.name] = Instantiate(prefabForTags, Vector3.zero, Quaternion.identity, parentForTags).GetComponent<TagScript>();
            AddCondition(s, conditionTags[s.condition.name]);
            Debug.Log(s.condition.name);
            conditionTags[s.condition.name].SetText(s);
        }
        //Debug.Log("Iterating");
    }

    public void AddConditionToMember()
    {
        TagScript temp = null;
        int tempN;

        if (!string.IsNullOrEmpty(conditionEnterer.text))
        {

            if (!int.TryParse(conditionAgeEnterer.text, out tempN))
            {
                tempN = 0;
            }

            if (string.IsNullOrEmpty(conditionDetailEnterer.text))
                conditionDetailEnterer.text = "";

            selectedMember.AddCondition(new ConditionInstanceClass(ConditionGlossary.GetCondition(conditionEnterer.text), conditionDetailEnterer.text, tempN));
        }
        else
        {
            LogError("You must specify the condition!");
        }
    }

    public void AddCondition(ConditionInstanceClass con, TagScript tagObject)
    {
        if (tagObject != null )
        {
            if (!conditionTags.ContainsKey(con.condition.name))
            {
                tagObject.condition = con;
                conditionTags.Add(con.condition.name, tagObject);
                conditionsBrowsed = false;
            }
        }
        else
        {
            Debug.Log("Tagscript no found");
        }
    }

    public void EnterCondition()
    {
        //AddCondition(conditionEnterer.text, null);
        if (selectedMember == null)
            CreateTempMember();

        AddConditionToMember();
        UpdateTagBox();
    }

    public void OpenConditionHistory()
    {
        condLisScript.SetConditionListing(selectedMember);
    }

    public void ClearTagBox()
    {
        conditionEnterer.text = "";
        conditionAgeEnterer.text = "";
        conditionDetailEnterer.text = "";

        Transform[] foundTags = parentForTags.GetComponentsInChildren<Transform>();

        for (int i = 1; i < foundTags.Length; i++)
        {
            Destroy(foundTags[i].gameObject);
        }
    }

    public void EliminateCondition(string tag)
    {
        Debug.Log("Eliminating");
        if (conditionTags.ContainsKey(tag))
        {
            Debug.Log("Hit");
            Destroy(conditionTags[tag].gameObject);
            conditionTags.Remove(tag);
            selectedMember.RemoveCondition(tag);
            UpdateTagBox();
        }
    }

    List<ConditionInstanceClass> CreateListFromDictionary()
    {
        List<ConditionInstanceClass> listOfStrings = new List<ConditionInstanceClass>();

        foreach(KeyValuePair<string, TagScript> s in conditionTags)
        {
            listOfStrings.Add(s.Value.condition);
        }

        return listOfStrings;
    }

    void FillDictionaryFromList(List<ConditionInstanceClass> strings)
    {
        conditionTags.Clear();
        foreach(ConditionInstanceClass s in strings)
        {
            conditionTags.Add(s.condition.name, null);
        }
    }

    public void DeleteMember()
    {
        FamilyManager.instance.DestroyWindow(selectedMember.ID);
        CloseFormulary();
    }
}
