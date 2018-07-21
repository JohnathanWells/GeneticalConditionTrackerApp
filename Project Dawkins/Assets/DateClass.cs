using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DateClass{
    public int month;
    public int day;
    public int year;

    public DateClass()
    {
        month = -1;
        day = -1;
        year = -1;
    }
    
    public DateClass(int m, int d, int y)
    {
        month = m;
        day = d;
        year = y;
    }

    public static bool ConvertStringsToDate(string month, string day, string year, out DateClass result)
    {
        int a, b, c;

        if (int.TryParse(month, out a) && int.TryParse(day, out b) && int.TryParse(year, out c))
        {
            result = new DateClass(a, b, c);
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}
