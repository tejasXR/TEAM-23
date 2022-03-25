using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AnnotationData
{
    public string text;
    public string author;
    public string timeStamp;
    public string screenShotName;

    public AnnotationData(string text, string author, string timeStamp)
    {
        this.text = text;
        this.author = author;
        this.timeStamp = timeStamp;
    }
}

public static class WriteToJson
{
    public static string jsonPath = "C:/Users/tejas/Documents/Personal Projects/MIT Reality Hack 2022/TEAM-23/Assets";
    
    /*public static void AnnotationToJson(AnnotationData annotationData)
    {
        var currentJson = JsonUtility.
        var annotation = JsonUtility.ToJson(annotationData);
        var path = Path.Combine(jsonPath, "Annotations.json");
        File.WriteAllText(path, annotation);
    }*/

    public static void AnnotationsToJson(List<Annotation> annotationsList)
    {
        string annotationString = "";

        foreach (var annotation in annotationsList)
        {
            annotationString += JsonUtility.ToJson(annotation.AnnotationData);
        }
        
        File.WriteAllText(jsonPath, annotationString);
    }
}