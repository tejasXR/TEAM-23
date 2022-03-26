using System;
using System.Collections;
using System.Collections.Generic;
using a11y_Wizards.Scripts;
using GoogleCloudStreamingSpeechToText;
using UnityEngine;
using VRKeys;

public class SystemsManager : MonoBehaviour
{
    [SerializeField] private AnnotationSystem annotationSystem;
    [SerializeField] private StreamingRecognizer streamingRecognizer;
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private HighResScreenShots screenshotTaker;

    [SerializeField] private GameObject writingCanvas;
    [SerializeField] private PreviewCanvas previewCanvas;


    /*[SerializeField] private SpeechToTextHelper flowStartHelper;
    [SerializeField] private SpeechToTextHelper confirmPreviewHelper;*/

    // private bool _flowStarted;
    // private bool _feedbackWritten;
    // private bool _feedbackSubmitted;
    public enum FlowState
    {
        AwaitingCall,
        WantsToInputFeedback,
        WantsToPreviewFeedback,
        WantsToSubmitFeedback
    }

    public FlowState flowState { get; private set; }

    private void OnEnable()
    {
        playerInput.AnnotationButtonPressedCallback += StartAnnotationFlow;
        // flowStartHelper.

        // streamingRecognizer.onInterimResult.AddListener(UpdateKeyboardDisplayText);
    }

    private void OnDisable()
    {
        playerInput.AnnotationButtonPressedCallback -= StartAnnotationFlow;
        // streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);
        // streamingRecognizer.onInterimResult.AddListener(System.OnSentenceUpdated);
    }

    public void StartAnnotationFlow()
    {
        if (flowState != FlowState.AwaitingCall) return;
        ChangeFlowState(FlowState.WantsToInputFeedback);
        StartCoroutine(AnnotationFlow());
    }

    private IEnumerator AnnotationFlow()
    {
        PositionKeyboard();
        screenshotTaker.CaptureScreenshot();
        StartCoroutine(EnableKeyboard());
        yield return new WaitUntil(() => flowState == FlowState.WantsToInputFeedback);
        WriteFeedback();
        streamingRecognizer.onInterimResult.AddListener(UpdateKeyboardDisplayText);
        yield return new WaitUntil(() => flowState == FlowState.WantsToPreviewFeedback);
        PreviewFeedback();
        yield return new WaitUntil(() => flowState == FlowState.WantsToSubmitFeedback);
        SubmitFeedback();
    }

    public void ChangeFlowState(FlowState newState)
    {
        flowState = newState;
        /*switch (newState)
        {
            case FlowState.AwaitingCall:
                // Subscribe to player button press
                playerInput.AnnotationButtonPressedCallback += StartAnnotationFlow;
                break;
            case FlowState.UserInputtingFeedback:
                // Unsubscribe to button press
                playerInput.AnnotationButtonPressedCallback -= StartAnnotationFlow;

                /*System.RegisterAll(
                    ("confirm", () => ChangeFlowState(FlowState.AnnotationPreview)),
                    ("cancel", () => ))#1#
                break;
            case FlowState.FeedbackPreview:
                break;
        
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }*/
    }

    private void UpdateKeyboardDisplayText(string s)
    {
        if (!keyboard.initialized) return;
        if (flowState != FlowState.WantsToInputFeedback) return;
        keyboard.SetText(keyboard.text + s);
    }

    private void PositionKeyboard()
    {
        var newPosition = playerInput.GetComponent<Player>().transform.forward + keyboard.positionRelativeToUser;
        keyboard.transform.position = newPosition;

        var playerDirection = playerInput.gameObject.transform.position;
        playerDirection.z = keyboard.transform.rotation.z;
        playerDirection.x = keyboard.transform.rotation.x;

        keyboard.transform.rotation = Quaternion.Euler(playerDirection.x, playerDirection.y, playerDirection.z);
    }
    
    public void Keyboard_EnterKeyPressed()
    {
        if (flowState == FlowState.WantsToInputFeedback)
            ChangeFlowState(FlowState.WantsToPreviewFeedback);
        else
        {
            ChangeFlowState(FlowState.WantsToSubmitFeedback);
        }
    }
    
    public void Keyboard_CancelKeyPressed()
    {
        CancelFeedback();
    }

    public void Voice_StartFeedback()
    {
        StartAnnotationFlow();
    }

    public void Voice_PreviewFeedback()
    {
        if (flowState != FlowState.WantsToInputFeedback) return;
        ChangeFlowState(FlowState.WantsToPreviewFeedback);
    }

    public void Voice_CancelFeedback()
    {
        CancelFeedback();
    }

    private void WriteFeedback()
    {
        writingCanvas.SetActive(true);
        previewCanvas.gameObject.SetActive(false);
    }

    private void PreviewFeedback()
    {
        writingCanvas.SetActive(false);
        previewCanvas.SetText(keyboard.displayText.text);
        previewCanvas.gameObject.SetActive(true);
    }

    private void SubmitFeedback()
    {
        var feedbackText = keyboard.displayText.text;
        annotationSystem.CreateAnnotation(feedbackText);

        ResetSystem();
    }

    private void CancelFeedback()
    {
        ResetSystem();
    }

    private void ResetSystem()
    {
        streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);

        StopAllCoroutines();
        keyboard.Disable();
        keyboard.SetText("");
        
        ChangeFlowState(FlowState.AwaitingCall);
    }

    
    private IEnumerator EnableKeyboard()
    {
        yield return new WaitUntil(() => keyboard.initialized);
        keyboard.Enable();
    }
}
