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

    public enum FlowState
    {
        Initialization,
        InitialPrompt,
        AwaitingUserCall,
        UserInput,
        AnnotationPreview
    }

    public FlowState flowState { get; private set; }

    private void OnEnable()
    {
        streamingRecognizer.onInterimResult.AddListener(UpdateKeyboardDisplayText);
    }

    private void OnDisable()
    {
        streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);
        // streamingRecognizer.onInterimResult.AddListener(System.OnSentenceUpdated);
    }

    public void StartAnnotationFlow()
    {
        StartCoroutine(EnableKeyboard());
        _flowStarted = true;
    }

    public void ChangeFlowState(FlowState newState)
    {
        switch (newState)
        {
            case FlowState.Initialization:
                break;
            case FlowState.InitialPrompt:
                break;
            case FlowState.AwaitingUserCall:
                break;
            case FlowState.UserInput:
                StartAnnotationFlow();
                /*System.RegisterAll(
                    ("confirm", () => ChangeFlowState(FlowState.AnnotationPreview)),
                    ("cancel", () => ))*/
                break;
            case FlowState.AnnotationPreview:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
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
