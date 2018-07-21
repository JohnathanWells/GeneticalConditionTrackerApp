using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public static class SaveLoad {

    static string filePath = Application.dataPath + "/FamilyFiles/" + "information.xml";

    public static bool CheckIfPathIsEmpty()
    {
        return !File.Exists(filePath);
    }

    public static void Save(List<FamilyMemberClass> list)
    {
        //Debug.Log(list.Count);
        string myXML = Serialize(list).ToString();
        Debug.Log(myXML);
        File.WriteAllText(filePath, myXML);
    }

    public static void Load(out List<FamilyMemberClass> list)
    {
        string myXML = File.ReadAllText(filePath);
        list = Deserialize<List<FamilyMemberClass>>(myXML);
    }

    public static StringWriter Serialize(object o)
    {
        var xs = new XmlSerializer(o.GetType());
        var xml = new StringWriter();
        xs.Serialize(xml, o);

        return xml;
    }

    public static T Deserialize<T>(string xml)
    {
        var xs = new XmlSerializer(typeof(T));
        return (T)xs.Deserialize(new StringReader(xml));
    }

}
