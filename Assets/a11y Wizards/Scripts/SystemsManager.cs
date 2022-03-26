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

    private bool _flowStarted;

    /*private void Start()
    {
        StartAnnotationFlow();
    }*/

    private void OnEnable()
    {
        streamingRecognizer.onInterimResult.AddListener(UpdateKeyboardDisplayText);
    }

    private void OnDisable()
    {
        streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);
    }

    public void StartAnnotationFlow()
    {
        StartCoroutine(EnableKeyboard());
        _flowStarted = true;
    }

    public void UpdateKeyboardDisplayText(string s)
    {
        if (!keyboard.initialized) return;
        if (!_flowStarted) return;
        keyboard.SetText(s);
    }

    public void CancelFeedback()
    {
        keyboard.Disable();
        _flowStarted = false;
    }

    public void SubmitFeedback()
    {
        keyboard.Disable();
        
        var feedbackText = keyboard.displayText.text;
        annotationSystem.CreateAnnotation(feedbackText);

        _flowStarted = false;
    }

    private IEnumerator EnableKeyboard()
    {
        yield return new WaitUntil(() => keyboard.initialized);
        keyboard.Enable();
    }
}
