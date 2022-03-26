using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyData
{
    public string timestamp;
    public List<string> imgs;
    public string annotation;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
