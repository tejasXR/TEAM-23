using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnnotationData
{
    public string text;
    public string author;
    public string timeStamp;

    public AnnotationData(string text, string author, string timeStamp)
    {
        this.text = text;
        this.author = author;
        this.timeStamp = timeStamp;
    }
}

public static class WriteToJson
{
    public static void AnnotationToJson(AnnotationData annotationData)
    {
        var annotation = JsonUtility.ToJson(annotationData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Annotations.json", annotation);
    }
}
