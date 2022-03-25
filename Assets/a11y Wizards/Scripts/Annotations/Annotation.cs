using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Annotation : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject writingContainer;
    [SerializeField] private GameObject successContainer;
    

    private AnnotationData _annotationData;
    public AnnotationData AnnotationData => _annotationData;

    public string Sentence => text.text;

    public void Initialize(string annotationText, Vector3 position)
    {
        text.text = annotationText;
        Position(position);
        successContainer.SetActive(false);
    }
    
    public void Initialize(Sprite annotationSprite, Vector3 position)
    {
        image.sprite = annotationSprite;
        Position(position);
    }

    public void SetAnnotationData(AnnotationData annotationData)
    {
        _annotationData = annotationData;
    }

    public void Complete()
    {
        writingContainer.SetActive(false);
        successContainer.SetActive(true);
    }

    public void UpdateText(string newText)
    {
        text.text = newText;
    }

    private void Position(Vector3 position)
    {
        transform.position = position;
    }
    
    
}
