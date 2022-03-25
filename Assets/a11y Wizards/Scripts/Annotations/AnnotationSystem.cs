using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationSystem : MonoBehaviour
{
    [SerializeField] private Annotation annotationPrefab;
    [SerializeField] private string currentAuthor;

    private List<Annotation> _annotations = new List<Annotation>();
    private Annotation _currentAnnotation;

    public bool IsCreatingAnnotation => _currentAnnotation != null;

    public void CreateAnnotation(string annotationText, Vector3 position)
    {
        Debug.Log("Creating annotation");
        
        var annotation = Instantiate(annotationPrefab);
        annotation.Initialize(annotationText, position);
        
        _currentAnnotation = annotation;
        
        if (!_annotations.Contains(_currentAnnotation))
            _annotations.Add(_currentAnnotation);
    }

    public void UpdateCurrentAnnotation(string newText)
    {
        if (_currentAnnotation == null)
            CreateAnnotation("", Vector3.forward);
        
        _currentAnnotation.UpdateText(newText);
    }

    public void CompleteAnnotationCreation()
    {
        var annotationData = new AnnotationData(_currentAnnotation.Sentence, currentAuthor, 
            $"{System.DateTime.Now.ToShortDateString()} at {System.DateTime.Now.ToLongTimeString()}" );
        
        _currentAnnotation.SetAnnotationData(annotationData);
        
        _currentAnnotation = null;
        Debug.Log("Completing annotation creation");
    }

    private void OnApplicationQuit()
    {
        WriteToJson.AnnotationsToJson(_annotations);
    }
}