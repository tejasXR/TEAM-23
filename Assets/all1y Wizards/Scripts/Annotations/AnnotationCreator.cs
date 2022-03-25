using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationCreator : MonoBehaviour
{
    [SerializeField] private Annotation annotationPrefab;
    [SerializeField] private string currentAuthor;
    
    public void CreateAnnotation(string annotationText, Vector3 position)
    {
        var annotation = Instantiate(annotationPrefab);
        annotation.Initialize(annotationText, position);

        var annotationData = new AnnotationData(annotationText, currentAuthor, System.DateTime.Now.ToShortDateString());
        WriteToJson.AnnotationToJson(annotationData);
    }
}