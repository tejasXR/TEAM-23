using System.Collections;
using System.Collections.Generic;
using GoogleCloudStreamingSpeechToText;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnnotationSystem : MonoBehaviour
{
    [SerializeField] private Annotation annotationPrefab;
    [SerializeField] private string currentAuthor;
    
    private Annotation _currentAnnotation;

    [Button("Create Annotation")]
    public void SampleAnnotation()
    {
        CreateAnnotation("hello!", Vector3.forward);
    }
    
    public void CreateAnnotation(string annotationText, Vector3 position)
    {
        var annotation = Instantiate(annotationPrefab);
        annotation.Initialize(annotationText, position);
        _currentAnnotation = annotation;

        var annotationData = new AnnotationData(annotationText, currentAuthor, $"{System.DateTime.Now.ToShortDateString()} at {System.DateTime.Now.ToLongTimeString()}" );
        WriteToJson.AnnotationToJson(annotationData);
    }

    public void UpdateCurrentAnnotation(string newText)
    {
        _currentAnnotation.UpdateText(newText);
    }

    public void CompleteAnnotationCreation()
    {
        _currentAnnotation = null;
    }
}