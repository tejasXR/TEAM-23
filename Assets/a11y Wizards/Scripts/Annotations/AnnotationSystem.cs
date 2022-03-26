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

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    public bool IsCreatingAnnotation => _currentAnnotation != null;

    public void CreateAnnotation(string annotationText)
    {
        if (_currentAnnotation != null)
            CompleteAnnotationCreation();

        var position = RaycastUtility.RaycastPosition(_player.PlayerHead.transform.position,
            _player.PlayerHead.transform.forward);
        
        Debug.Log($"Creating annotation at {position.ToString("f4")}");

        var annotation = Instantiate(annotationPrefab);
        annotation.Initialize(annotationText, position);
        
        _currentAnnotation = annotation;
        
        if (!_annotations.Contains(_currentAnnotation))
            _annotations.Add(_currentAnnotation);
    }

    public void UpdateCurrentAnnotation(string newText)
    {
        if (_currentAnnotation == null)
            CreateAnnotation("");
        
        Debug.Log("Updating annotation text");
        
        _currentAnnotation.UpdateText(newText);
    }

    public void CompleteAnnotationCreation()
    {
        var annotationData = new AnnotationData(_currentAnnotation.Sentence, currentAuthor, 
            $"{System.DateTime.Now.ToShortDateString()} at {System.DateTime.Now.ToLongTimeString()}" );
        
        _currentAnnotation.SetAnnotationData(annotationData);
        _currentAnnotation.Complete();
        
        _currentAnnotation = null;
        Debug.Log("Completing annotation creation");
    }

    private void OnApplicationQuit()
    {
        WriteToJson.AnnotationsToJson(_annotations);
    }
}