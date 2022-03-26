using System;
using System.Collections;
using System.Collections.Generic;
using GoogleCloudStreamingSpeechToText;
using UnityEngine;
using VRKeys;

public class SystemsManager : MonoBehaviour
{
    [SerializeField] private AnnotationSystem annotationSystem;
    [SerializeField] private StreamingRecognizer streamingRecognizer;
    [SerializeField] private Keyboard keyboard;

    /*private void Start()
    {
        StartAnnotationFlow();
    }*/

    public void StartAnnotationFlow()
    {
        StartCoroutine(EnableKeyboard());
    }

    public void FeedbackSubmitted(string s)
    {
        keyboard.Disable();
        annotationSystem.CreateAnnotation(s);
    }
    
    private IEnumerator EnableKeyboard()
    {
        yield return new WaitUntil(() => keyboard.initialized);
        keyboard.Enable();
    }
}
