using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnnotationData
{
    public string annotationAuthor;
    public string annotationText;
    public string annotationTimeStamp;
}

public class WriteToJSON
{
    public static void AnnotationToJSON(AnnotationData annotationData)
    {
        var annotation = JsonUtility.ToJson(annotationData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Annotations.json", annotation);
    }
}
